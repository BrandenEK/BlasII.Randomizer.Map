using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;

namespace BlasII.Randomizer.Map
{
    public static class Data
    {
        public static ImmutableDictionary<Vector2, string[]> MapLocations => _mapLocations.ToImmutableDictionary();

        private static readonly Dictionary<Vector2, string[]> _mapLocations = new()
        {
            // Repose of the Silent One
            { new Vector2(23, 43), new string[] { "Location" } },
            { new Vector2(28, 43), new string[] { "Location" } },
            { new Vector2(29.5f, 42), new string[] { "Location" } },

            // Ravine of the High Stones
            { new Vector2(39, 42), new string[] { "Location" } },
            { new Vector2(38, 43), new string[] { "Location" } },
            { new Vector2(38, 44), new string[] { "Location" } },
            { new Vector2(42, 43), new string[] { "Location" } },
            { new Vector2(43, 43), new string[] { "Location" } },
            { new Vector2(42, 44), new string[] { "Location" } },

            // Sacred Entombments
            { new Vector2(28, 49), new string[] { "Location" } },

            // Debug
            { new Vector2(40, 35), new string[] { "Location" } },
        };
    }
}
