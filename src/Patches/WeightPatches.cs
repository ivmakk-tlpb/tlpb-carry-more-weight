using System;
using System.Globalization;
using HarmonyLib;

namespace CarryMoreWeight.Patches
{
    // The item weight. InventoryManager.initializeDisplayProperties runs for each item at the item load and again at each
    // language change, and it sets the "Heavy" and "Very Heavy" tooltip lines from the weight. The prefix writes the
    // divided weight first, so the tooltip lines and the carried weight of InventoryHolder.Update read the same value.
    [HarmonyPatch]
    internal static class WeightPatches
    {
        private const string Where = "Item weight";

        private static WeightRule rule = new WeightRule();

        internal static void Reset() => rule = new WeightRule();

        [HarmonyPrefix]
        [HarmonyPatch(typeof(InventoryManager), "initializeDisplayProperties", new[] { typeof(InventoryItem) })]
        internal static void Divide(InventoryItem item)
        {
            try
            {
                if (item == null)
                {
                    Plugin.Debug("Item weight: no item");
                    return;
                }
                string identifier = item.identifier.Value;
                var field = item.weight;
                if (field == null)
                {
                    Plugin.Debug("Item weight " + identifier + ": no weight field");
                    return;
                }
                float current = field.Value;
                if (!rule.TryNewWeight(identifier, current, Plugin.Divisor, out float game, out float weight) || weight == current) return;
                field.Value = weight;
                Plugin.Debug("Item weight " + identifier + ": " + Text(game) + " -> " + Text(weight));
            }
            catch (Exception e)
            {
                Plugin.WarnOnce(Where, e);
            }
        }

        private static string Text(float value) => value.ToString("0.####", CultureInfo.InvariantCulture);
    }
}
