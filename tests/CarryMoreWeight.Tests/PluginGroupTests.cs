using CarryMoreWeight;
using Xunit;

public class PluginGroupTests : GameTestBase
{
    private static class StubPatches { }

    private static class OtherStubPatches { }

    [Fact]
    public void A_group_is_applied_with_its_own_harmony_id()
    {
        Plugin.Apply(new Plugin.PatchGroup("stub", "off", typeof(StubPatches), typeof(OtherStubPatches)));

        Assert.Contains(typeof(StubPatches), HarmonyLib.Harmony.Patched);
        Assert.Contains(typeof(OtherStubPatches), HarmonyLib.Harmony.Patched);
        Assert.Contains("stub", Plugin.Applied);
        Assert.Empty(Plugin.Log.Warnings);
    }

    [Fact]
    public void A_missing_target_removes_only_that_group_with_one_warning()
    {
        HarmonyLib.Harmony.ThrowFor = typeof(StubPatches);

        Plugin.Apply(new Plugin.PatchGroup("stub", "Carry More Weight leaves the stub as in the game", typeof(StubPatches)));
        Plugin.Apply(new Plugin.PatchGroup("other", "off", typeof(OtherStubPatches)));

        Assert.Equal(new[] { Plugin.PluginGuid + ".stub" }, HarmonyLib.Harmony.Unpatched);
        Assert.Equal(new[] { "Carry More Weight leaves the stub as in the game: Target not found" }, Plugin.Log.Warnings);
        Assert.DoesNotContain("stub", Plugin.Applied);
        Assert.Contains("other", Plugin.Applied);
    }

    private static class FailingPreparePatches
    {
        [HarmonyLib.HarmonyPrepare]
        internal static void Prepare() => throw new MissingMemberException("InventoryItem.Tags");
    }

    [Fact]
    public void A_failed_prepare_check_removes_only_that_group_with_one_warning()
    {
        Plugin.Applied.Clear();

        Plugin.Apply(new Plugin.PatchGroup("hand", "Carry More Weight leaves the hand slowdown as in the game", typeof(FailingPreparePatches)));
        Plugin.Apply(new Plugin.PatchGroup("item weight", "off", typeof(StubPatches)));

        Assert.Equal(new[] { "Carry More Weight leaves the hand slowdown as in the game: InventoryItem.Tags" }, Plugin.Log.Warnings);
        Assert.DoesNotContain("hand", Plugin.Applied);
        Assert.Contains("item weight", Plugin.Applied);
    }

    [Fact]
    public void The_verbose_line_names_an_applied_group()
    {
        Plugin.Verbose.Value = true;

        Plugin.Apply(new Plugin.PatchGroup("stub", "off", typeof(StubPatches)));

        Assert.Contains("Patch group stub applied", Plugin.Log.Debugs);
    }
}
