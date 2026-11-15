using System.Collections.Generic;

namespace CarryMoreWeight
{
    // The item weight rule: the game weight of an item kind divided by the divisor of the setting.
    internal sealed class WeightRule
    {
        internal const float HighestDivisor = 20f;

        // The game weight of each item kind, kept at the first call, because a later call (a language change) passes
        // the item with the divided weight.
        private readonly Dictionary<string, float> gameWeights = new Dictionary<string, float>();

        // 0 or less, or NaN, gives 0, so each item weighs nothing. A value between 0 and 1 gives 1, so an item never gets
        // heavier than in the game. A value above 20 gives 20.
        internal static float Divisor(float setting)
        {
            if (!(setting > 0f)) return 0f;
            if (setting < 1f) return 1f;
            return setting > HighestDivisor ? HighestDivisor : setting;
        }

        // False for an item kind that weighs nothing in the game: it keeps its weight of 0.
        internal bool TryNewWeight(string identifier, float current, float divisor, out float game, out float weight)
        {
            if (!gameWeights.TryGetValue(identifier, out game))
            {
                game = current;
                gameWeights[identifier] = game;
            }
            weight = divisor <= 0f ? 0f : game / divisor;
            return game > 0f;
        }
    }
}
