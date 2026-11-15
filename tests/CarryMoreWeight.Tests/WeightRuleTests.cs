using CarryMoreWeight;
using Xunit;

public class WeightRuleTests
{
    [Theory]
    [InlineData(0f, 0f)]
    [InlineData(0.5f, 1f)]
    [InlineData(-1f, 0f)]
    [InlineData(float.NaN, 0f)]
    [InlineData(2f, 2f)]
    [InlineData(50f, 20f)]
    public void The_divisor_follows_the_setting_within_its_limits(float setting, float divisor)
    {
        Assert.Equal(divisor, WeightRule.Divisor(setting));
    }

    private readonly WeightRule rule = new WeightRule();

    private float New(string identifier, float current, float divisor)
    {
        Assert.True(rule.TryNewWeight(identifier, current, divisor, out _, out float weight));
        return weight;
    }

    [Fact]
    public void Iron_ore_weighs_half_at_divisor_2()
    {
        Assert.Equal(2f, New("iron_ore", 4f, 2f));
    }

    [Fact]
    public void A_second_call_with_the_divided_weight_divides_the_game_weight_once()
    {
        New("iron_ore", 4f, 2f);

        Assert.Equal(2f, New("iron_ore", 2f, 2f));
        Assert.True(rule.TryNewWeight("iron_ore", 2f, 2f, out float game, out _));
        Assert.Equal(4f, game);
    }

    [Fact]
    public void Divisor_0_gives_weight_0()
    {
        Assert.Equal(0f, New("firewood", 20f, 0f));
    }

    [Fact]
    public void Divisor_1_gives_the_game_weight()
    {
        Assert.Equal(4f, New("iron_ore", 4f, 1f));
    }

    [Fact]
    public void An_item_with_no_weight_reports_no_change()
    {
        Assert.False(rule.TryNewWeight("berries", 0f, 2f, out _, out _));
    }

    [Fact]
    public void A_new_divisor_between_two_calls_uses_the_kept_game_weight()
    {
        New("firewood", 20f, 2f);

        Assert.Equal(5f, New("firewood", 10f, 4f));
        Assert.Equal(20f, New("firewood", 0f, 1f));
    }

    [Fact]
    public void Each_item_kind_keeps_its_own_game_weight()
    {
        New("iron_ore", 4f, 2f);

        Assert.Equal(10f, New("firewood", 20f, 2f));
    }
}
