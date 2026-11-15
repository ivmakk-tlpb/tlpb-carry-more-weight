using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace CarryMoreWeight
{
    [BepInPlugin(PluginGuid, "Carry More Weight", "1.0.0")]
    [BepInProcess("TheLastPlague.exe")]
    public sealed class Plugin : BasePlugin
    {
        public const string PluginGuid = "com.ivmakk.thelastplagueblight.carrymoreweight";

        internal static new ManualLogSource Log;
        internal static ConfigEntry<float> WeightDivisor;
        internal static ConfigEntry<bool> HandSlowdown;
        internal static ConfigEntry<bool> Verbose;

        // A patch group is applied alone with its own Harmony id: a missing target removes only that group.
        internal sealed class PatchGroup
        {
            public readonly string Name;
            public readonly string OffText;
            public readonly List<Type> Types;

            public PatchGroup(string name, string offText, params Type[] types)
            {
                Name = name;
                OffText = offText;
                Types = new List<Type>(types);
            }
        }

        internal static readonly List<PatchGroup> Groups = new List<PatchGroup>
        {
            new PatchGroup("item weight", "Carry More Weight leaves the item weights as in the game", typeof(Patches.WeightPatches)),
            new PatchGroup("hand slowdown", "Carry More Weight leaves the hand slowdown as in the game", typeof(Patches.HandPatches)),
        };

        private static readonly HashSet<string> warned = new HashSet<string>();

        // The names of the patch groups that are in place.
        internal static readonly HashSet<string> Applied = new HashSet<string>();

        // The divisor of the setting, limited when it is read, also after an edit of the config file by hand.
        internal static float Divisor => WeightRule.Divisor(WeightDivisor.Value);

        public override void Load()
        {
            Log = base.Log;
            warned.Clear();
            Applied.Clear();
            WeightDivisor = Config.Bind(
                "General", "WeightDivisor", 2f,
                new ConfigDescription(
                    "How many times lighter heavy items are, such as ores, ingots, bricks, planks, Firewood, logs, and lumber. 2 is half the weight of the game, so you carry twice as much before you get Encumbered. 1 is as in the game, and 20 is the highest. A value between 0 and 1 counts as 1. 0 means items weigh nothing and you never get Encumbered. A new value applies after a restart of the game.",
                    new AcceptableValueRange<float>(0f, WeightRule.HighestDivisor)));
            HandSlowdown = Config.Bind(
                "General", "HandSlowdown", false,
                "Run at half speed while you hold a log, a trunk, lumber, Firewood, an anvil, or another large heavy item in your hands, as in the game. false removes this slowdown, and only Encumbered slows you. A new value applies after a restart of the game.");
            Verbose = Config.Bind(
                "General", "Verbose", false,
                "Writes extra diagnostic lines to the BepInEx log for troubleshooting. Keep off in normal play.");
            foreach (var group in Groups) Apply(group);
            Log.LogInfo("Carry More Weight loaded.");
        }

        // A game update can rename a target. Then the patches of that group go, and the other groups stay.
        internal static void Apply(PatchGroup group)
        {
            var harmony = new Harmony(PluginGuid + "." + group.Name);
            try
            {
                foreach (var patches in group.Types) harmony.PatchAll(patches);
                Applied.Add(group.Name);
                Debug("Patch group " + group.Name + " applied");
            }
            catch (Exception e)
            {
                harmony.UnpatchSelf();
                Log.LogWarning(group.OffText + ": " + e.Message);
            }
        }

        // One warning for each distinct error, so a repeated error does not flood the log.
        internal static void WarnOnce(string where, Exception e)
        {
            string text = where + ": " + e.GetType().Name + ": " + e.Message;
            if (warned.Add(text)) Log.LogWarning(text + "\n" + e);
        }

        internal static void Debug(string line)
        {
            if (Verbose.Value) Log.LogDebug(line);
        }
    }
}
