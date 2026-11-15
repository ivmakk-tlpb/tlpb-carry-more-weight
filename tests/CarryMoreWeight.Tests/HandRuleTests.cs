using CarryMoreWeight;
using Xunit;

public class HandRuleTests
{
    private readonly HandRule rule = new HandRule();

    [Fact]
    public void A_log_loses_its_tag_at_the_default()
    {
        Assert.Equal(HandChange.Remove, rule.Decide("log_thin", true, false));
    }

    [Fact]
    public void A_second_call_after_the_removal_changes_nothing()
    {
        rule.Decide("log_thin", true, false);

        Assert.Equal(HandChange.None, rule.Decide("log_thin", false, false));
    }

    [Fact]
    public void Keeping_the_slowdown_puts_a_removed_tag_back()
    {
        rule.Decide("log_thin", true, false);

        Assert.Equal(HandChange.AddBack, rule.Decide("log_thin", false, true));
    }

    [Fact]
    public void Keeping_the_slowdown_leaves_a_tagged_item_as_it_is()
    {
        Assert.Equal(HandChange.None, rule.Decide("log_thin", true, true));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void An_item_with_no_tag_in_the_game_never_changes(bool keep)
    {
        Assert.Equal(HandChange.None, rule.Decide("berries", false, keep));
        Assert.Equal(HandChange.None, rule.Decide("berries", false, !keep));
    }
}
