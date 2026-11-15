using System.Collections.Generic;

namespace CarryMoreWeight
{
    internal enum HandChange
    {
        None,
        Remove,
        AddBack,
    }

    // The hand slowdown rule: whether an item kind keeps the game tag that slows a character with that item in its hands.
    internal sealed class HandRule
    {
        // Whether each item kind has the tag in the game, kept at the first call, because a later call (a language change)
        // passes the item after the mod removed the tag.
        private readonly Dictionary<string, bool> gameTags = new Dictionary<string, bool>();

        internal HandChange Decide(string identifier, bool hasTag, bool keepSlowdown)
        {
            if (!gameTags.TryGetValue(identifier, out bool game))
            {
                game = hasTag;
                gameTags[identifier] = game;
            }
            if (!game) return HandChange.None;
            if (keepSlowdown) return hasTag ? HandChange.None : HandChange.AddBack;
            return hasTag ? HandChange.Remove : HandChange.None;
        }
    }
}
