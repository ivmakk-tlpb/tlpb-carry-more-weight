using BepInEx.Configuration;
using CarryMoreWeight;
using Xunit;

public class PluginTests : GameTestBase
{
    [Fact]
    public void The_guid_names_the_mod()
    {
        Assert.Equal("com.ivmakk.thelastplagueblight.carrymoreweight", Plugin.PluginGuid);
    }

    [Fact]
    public void Debug_lines_are_off_by_default()
    {
        Assert.False(Plugin.Verbose.Value);
    }

    [Fact]
    public void The_weight_divisor_has_the_default_2_and_the_range_0_to_20()
    {
        Assert.Equal(2f, Plugin.WeightDivisor.Value);
        var range = (AcceptableValueRange<float>)Plugin.WeightDivisor.Description.AcceptableValues;
        Assert.Equal(0f, range.MinValue);
        Assert.Equal(20f, range.MaxValue);
    }

    [Fact]
    public void The_hand_slowdown_is_off_by_default()
    {
        Assert.False(Plugin.HandSlowdown.Value);
    }

    [Fact]
    public void Both_patch_groups_are_applied_at_load()
    {
        Assert.Contains("item weight", Plugin.Applied);
        Assert.Contains("hand slowdown", Plugin.Applied);
    }

    [Fact]
    public void The_setting_is_read_as_its_divisor()
    {
        Plugin.WeightDivisor.Value = 0.5f;

        Assert.Equal(1f, Plugin.Divisor);
    }

    [Fact]
    public void The_debug_helper_writes_only_while_verbose_is_on()
    {
        Plugin.Debug("hidden");
        Plugin.Verbose.Value = true;
        Plugin.Debug("shown");

        Assert.Contains("shown", Plugin.Log.Debugs);
        Assert.DoesNotContain("hidden", Plugin.Log.Debugs);
    }

    [Fact]
    public void The_plugin_logs_that_it_loaded_with_no_warning()
    {
        Assert.Contains("Carry More Weight loaded.", Plugin.Log.Infos);
        Assert.Empty(Plugin.Log.Warnings);
    }

    [Fact]
    public void A_repeated_error_gives_one_warning()
    {
        Plugin.WarnOnce("Part", new InvalidOperationException("broken"));
        Plugin.WarnOnce("Part", new InvalidOperationException("broken"));

        Assert.Single(Plugin.Log.Warnings);
    }
}
