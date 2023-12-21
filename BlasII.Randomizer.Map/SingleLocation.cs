﻿using BlasII.Randomizer.Items;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    internal class SingleLocation : ILocation
    {
        private readonly string _id;

        // Stored when the map UI is created
        public Image Image { get; set; }

        public SingleLocation(string id) => _id = id;

        public Logic GetReachability(Blas2Inventory inventory)
        {
            if (IsCollected)
                return Logic.Finished;

            int rand = Random.Range(1, 4);
            return rand switch
            {
                0 => Logic.Finished,
                1 => Logic.NoneReachable,
                2 => Logic.SomeReachable,
                _ => Logic.AllReachable,
            };
        }

        private bool IsCollected => Main.Randomizer.ItemHandler.CollectedLocations.Contains(_id);
    }
}
