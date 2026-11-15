using HarmonyLib;

// Puts the static state of the fake game types and the patches back to a known start.
internal static class FakeState
{
    public static void Reset()
    {
        CarryMoreWeight.Patches.WeightPatches.Reset();
        CarryMoreWeight.Patches.HandPatches.Reset();
        Harmony.Patched.Clear();
        Harmony.Unpatched.Clear();
        Harmony.ThrowFor = null;
    }
}
