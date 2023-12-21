using BlasII.Randomizer.Items;
using System.Linq;

namespace BlasII.Randomizer.Map
{
    internal class InventoryHandler
    {
        private Blas2Inventory _currentInventory;
        private bool _needsRefresh = true;

        public Blas2Inventory CurrentInventory
        {
            get
            {
                if (_needsRefresh)
                {
                    _needsRefresh = false;
                    CalculateInventory();
                }

                return _currentInventory;
            }
        }

        /// <summary>
        /// Force the inventory to be recalculated next time it is needed
        /// </summary>
        public void Refresh() => _needsRefresh = true;

        /// <summary>
        /// Recalculate the inventory of the playthrough
        /// </summary>
        private void CalculateInventory()
        {
            Main.MapTracker.Log("Calculating new inventory");
            _currentInventory = new Blas2Inventory(Main.Randomizer.CurrentSettings, Main.Randomizer.Data.DoorDictionary);

            foreach (var item in Main.Randomizer.Data.ItemList)
            {
                // Only check progression items
                if (!item.progression)
                    continue;

                // Find the amount the player has of this item
                int amount;
                if (item.subItems == null)
                {
                    amount = Main.Randomizer.ItemHandler.CollectedItems.Count(x => x == item.id);
                }
                else
                {
                    amount = item.subItems.Count(x => Main.Randomizer.ItemHandler.IsItemCollected(x));
                }

                // Add it to the inventory
                Main.MapTracker.LogWarning($"Adding {item.id} {amount} times!");
                for (int i = 0; i < amount; i++)
                {
                    _currentInventory.AddItem(item);
                }
            }
        }
    }
}
