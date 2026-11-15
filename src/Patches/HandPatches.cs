using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace CarryMoreWeight.Patches
{
    // The hand slowdown. EntityMovementView.Move caps the speed of a character at half its top speed while an item with
    // the tag HEAVY is in its hands, and it is the only reader of that tag in the game. The prefix removes the tag from
    // each item kind at the item load, so no per-frame work is needed. It runs on the same game method as the weight
    // prefix, so Harmony runs both in one replacement method: the prepare check and the helper below keep a failure of
    // this prefix away from the weight prefix and the game method.
    [HarmonyPatch]
    internal static class HandPatches
    {
        private const string Where = "Hand slowdown";

        private static HandRule rule = new HandRule();

        internal static void Reset() => rule = new HandRule();

        // A game update that removes a member fails the patch of this group, so the plugin logs one warning and leaves
        // the group out.
        [HarmonyPrepare]
        internal static void Prepare()
        {
            string missing = MissingMember(typeof(InventoryItem), typeof(ItemTag));
            if (missing != null) throw new MissingMemberException(missing);
        }

        internal static string MissingMember(Type item, Type tag)
        {
            if (item.GetProperty("Tags", BindingFlags.Instance | BindingFlags.Public) == null) return item.Name + ".Tags";
            if (!Enum.IsDefined(tag, "HEAVY")) return tag.Name + ".HEAVY";
            return null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryManager), "initializeDisplayProperties", new[] { typeof(InventoryItem) })]
        internal static void Change(InventoryItem item)
        {
            try
            {
                if (item == null)
                {
                    Plugin.Debug("Hand slowdown: no item");
                    return;
                }
                ChangeTag(item);
            }
            catch (Exception e)
            {
                Plugin.WarnOnce(Where, e);
            }
        }

        // The game members are reached only here, so a member that a game update removed fails the compile of this
        // method inside the try of the prefix.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ChangeTag(InventoryItem item)
        {
            string identifier = item.identifier.Value;
            var tags = item.Tags;
            if (tags == null)
            {
                Plugin.Debug("Hand slowdown " + identifier + ": no tag set");
                return;
            }
            switch (rule.Decide(identifier, tags.Contains(ItemTag.HEAVY), Plugin.HandSlowdown.Value))
            {
                case HandChange.Remove:
                    tags.Remove(ItemTag.HEAVY);
                    Plugin.Debug("Hand slowdown " + identifier + ": removed");
                    break;
                case HandChange.AddBack:
                    tags.Add(ItemTag.HEAVY);
                    Plugin.Debug("Hand slowdown " + identifier + ": put back");
                    break;
            }
        }
    }
}
