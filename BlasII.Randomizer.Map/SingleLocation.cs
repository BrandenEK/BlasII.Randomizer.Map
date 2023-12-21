﻿using BlasII.Randomizer.Items;
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

            ItemLocation location = Main.Randomizer.Data.GetItemLocation(_id);
            return inventory.Evaluate(location.logic) ? Logic.AllReachable : Logic.NoneReachable;
        }

        private bool IsCollected => Main.Randomizer.ItemHandler.CollectedLocations.Contains(_id);
    }
}
