using CarryMoreWeight;
using CarryMoreWeight.Patches;
using Xunit;

public class WeightPatchesTests : GameTestBase
{
    private static float Load(InventoryItem item)
    {
        WeightPatches.Divide(item);
        return item.weight.Value;
    }

    [Fact]
    public void Iron_ore_weighs_half_at_the_default()
    {
        Assert.Equal(2f, Load(new InventoryItem("iron_ore", 4f)));
    }

    [Fact]
    public void A_language_change_does_not_divide_the_weight_again()
    {
        var ore = new InventoryItem("iron_ore", 4f);
        Load(ore);

        Assert.Equal(2f, Load(ore));
    }

    [Fact]
    public void At_divisor_0_an_item_weighs_nothing()
    {
        Plugin.WeightDivisor.Value = 0f;

        Assert.Equal(0f, Load(new InventoryItem("firewood", 20f)));
    }

    [Fact]
    public void A_new_config_value_applies_at_the_next_call()
    {
        var firewood = new InventoryItem("firewood", 20f);
        Load(firewood);
        Plugin.WeightDivisor.Value = 1f;

        Assert.Equal(20f, Load(firewood));
    }

    [Fact]
    public void An_item_that_weighs_nothing_gets_no_write_and_no_line()
    {
        Plugin.Verbose.Value = true;
        var berries = new InventoryItem("berries", 0f);

        Assert.Equal(0f, Load(berries));
        Assert.DoesNotContain(Plugin.Log.Debugs, l => l.Contains("berries"));
    }

    [Fact]
    public void Each_changed_item_kind_gets_one_verbose_line()
    {
        Plugin.Verbose.Value = true;
        var ore = new InventoryItem("iron_ore", 4f);

        Load(ore);
        Load(ore);
        Load(new InventoryItem("short_plank", 2.5f));

        Assert.Equal(new[] { "Item weight iron_ore: 4 -> 2", "Item weight short_plank: 2.5 -> 1.25" }, Plugin.Log.Debugs.Where(l => l.StartsWith("Item weight ")));
    }

    [Fact]
    public void A_missing_item_is_named_in_the_verbose_log()
    {
        Plugin.Verbose.Value = true;

        WeightPatches.Divide(null);

        Assert.Contains("Item weight: no item", Plugin.Log.Debugs);
        Assert.Empty(Plugin.Log.Warnings);
    }

    [Fact]
    public void A_missing_weight_field_is_named_in_the_verbose_log()
    {
        Plugin.Verbose.Value = true;

        WeightPatches.Divide(new InventoryItem("iron_ore", 4f) { weight = null });

        Assert.Contains("Item weight iron_ore: no weight field", Plugin.Log.Debugs);
        Assert.Empty(Plugin.Log.Warnings);
    }

    [Fact]
    public void An_error_keeps_the_game_weight_and_gives_one_warning()
    {
        var broken = new InventoryItem("iron_ore", 4f);
        broken.weight.Broken = true;

        WeightPatches.Divide(broken);
        WeightPatches.Divide(broken);

        Assert.Single(Plugin.Log.Warnings);
        broken.weight.Broken = false;
        Assert.Equal(4f, broken.weight.Value);
    }

    [Fact]
    public void The_weight_patches_are_in_the_item_weight_group()
    {
        Assert.Equal(new[] { typeof(WeightPatches) }, Plugin.Groups.Single(g => g.Name == "item weight").Types);
        Assert.Contains(typeof(WeightPatches), HarmonyLib.Harmony.Patched);
        Assert.Contains("item weight", Plugin.Applied);
    }
}
