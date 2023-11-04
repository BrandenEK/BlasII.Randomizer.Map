using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

namespace BlasII.Randomizer.Map
{
    public static class Data
    {
        public static ImmutableDictionary<Vector2, object> MapLocations => _mapLocations.ToImmutableDictionary();

        private static readonly Dictionary<Vector2, object> _mapLocations = new()
        {
            // Repose of the Silent One
            { new Vector2(23, 43), null },
            { new Vector2(28, 43), null },
            { new Vector2(29.5f, 42), null },

            // Ravine of the High Stones
            { new Vector2(39, 42), null },
            { new Vector2(38, 43), null },
            { new Vector2(38, 44), null },
            { new Vector2(42, 43), null },
            { new Vector2(43, 43), null },
            { new Vector2(42, 44), null },

            // Sacred Entombments
            { new Vector2(28, 49), null },

            // Debug
            { new Vector2(40, 35), null },
        };
    }
}
