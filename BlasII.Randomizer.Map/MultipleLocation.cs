﻿using BlasII.Randomizer.Items;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    internal class MultipleLocation : ILocation
    {
        private readonly string[] _ids;

        // Stored when the map UI is created
        public Image Image { get; set; }

        public MultipleLocation(string[] ids) => _ids = ids;

        public Logic GetReachability(Blas2Inventory inventory)
        {
            int rand = Random.Range(0, 4);
            return rand switch
            {
                0 => Logic.Finished,
                1 => Logic.NoneReachable,
                2 => Logic.SomeReachable,
                _ => Logic.AllReachable,
            };
        }
    }
}