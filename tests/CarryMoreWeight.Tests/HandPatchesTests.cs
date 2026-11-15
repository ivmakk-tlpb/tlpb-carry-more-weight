using CarryMoreWeight;
using CarryMoreWeight.Patches;
using Xunit;

public class HandPatchesTests : GameTestBase
{
    private static InventoryItem Log() => new InventoryItem("log_thin", 40f, ItemTag.HEAVY, ItemTag.LARGE);

    private static bool Heavy(InventoryItem item) => item.Tags.Contains(ItemTag.HEAVY);

    [Fact]
    public void A_log_loses_the_heavy_tag_at_the_default()
    {
        var log = Log();

        HandPatches.Change(log);

        Assert.False(Heavy(log));
        Assert.True(log.Tags.Contains(ItemTag.LARGE));
    }

    [Fact]
    public void A_language_change_keeps_the_tag_off_with_one_verbose_line()
    {
        Plugin.Verbose.Value = true;
        var log = Log();

        HandPatches.Change(log);
        HandPatches.Change(log);

        Assert.False(Heavy(log));
        Assert.Equal(new[] { "Hand slowdown log_thin: removed" }, Plugin.Log.Debugs.Where(l => l.StartsWith("Hand slowdown ")));
    }

    [Fact]
    public void Keeping_the_slowdown_puts_the_tag_back()
    {
        Plugin.Verbose.Value = true;
        var log = Log();
        HandPatches.Change(log);
        Plugin.HandSlowdown.Value = true;

        HandPatches.Change(log);

        Assert.True(Heavy(log));
        Assert.Contains("Hand slowdown log_thin: put back", Plugin.Log.Debugs);
    }

    [Fact]
    public void Keeping_the_slowdown_from_the_start_leaves_the_tag()
    {
        Plugin.HandSlowdown.Value = true;
        var log = Log();

        HandPatches.Change(log);

        Assert.True(Heavy(log));
    }

    [Fact]
    public void An_item_with_no_heavy_tag_gets_no_change_and_no_line()
    {
        Plugin.Verbose.Value = true;
        var berries = new InventoryItem("berries", 0f);

        HandPatches.Change(berries);

        Assert.False(Heavy(berries));
        Assert.DoesNotContain(Plugin.Log.Debugs, l => l.StartsWith("Hand slowdown "));
    }

    [Fact]
    public void A_missing_item_is_named_in_the_verbose_log()
    {
        Plugin.Verbose.Value = true;

        HandPatches.Change(null);

        Assert.Contains("Hand slowdown: no item", Plugin.Log.Debugs);
        Assert.Empty(Plugin.Log.Warnings);
    }

    [Fact]
    public void A_missing_tag_set_is_named_in_the_verbose_log()
    {
        Plugin.Verbose.Value = true;

        HandPatches.Change(new InventoryItem("log_thin", 40f) { Tags = null });

        Assert.Contains("Hand slowdown log_thin: no tag set", Plugin.Log.Debugs);
        Assert.Empty(Plugin.Log.Warnings);
    }

    [Fact]
    public void An_error_gives_one_warning_and_the_weight_prefix_still_works()
    {
        var log = new InventoryItem("log_thin", 40f, ItemTag.HEAVY) { TagsBroken = true };

        HandPatches.Change(log);
        HandPatches.Change(log);
        WeightPatches.Divide(log);

        Assert.Single(Plugin.Log.Warnings);
        Assert.StartsWith("Hand slowdown: InvalidOperationException: broken tags", Plugin.Log.Warnings[0]);
        Assert.Equal(20f, log.weight.Value);
    }

    [Fact]
    public void The_game_members_of_this_version_pass_the_prepare_check()
    {
        Assert.Null(HandPatches.MissingMember(typeof(InventoryItem), typeof(ItemTag)));
        Assert.Contains("hand slowdown", Plugin.Applied);
    }

    private sealed class ItemWithNoTags { }

    private enum TagWithNoHeavy { LARGE = 11 }

    [Fact]
    public void The_prepare_check_names_a_missing_tag_set()
    {
        Assert.Equal("ItemWithNoTags.Tags", HandPatches.MissingMember(typeof(ItemWithNoTags), typeof(ItemTag)));
    }

    [Fact]
    public void The_prepare_check_names_a_missing_heavy_tag()
    {
        Assert.Equal("TagWithNoHeavy.HEAVY", HandPatches.MissingMember(typeof(InventoryItem), typeof(TagWithNoHeavy)));
    }
}
