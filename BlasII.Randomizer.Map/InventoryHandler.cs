using BlasII.Randomizer.Items;

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

        public void Refresh() => _needsRefresh = true;

        private void CalculateInventory()
        {
            Main.MapTracker.Log("Calculating new inventory");
            var randomizer = BlasII.Randomizer.Main.Randomizer;

            _currentInventory = new Blas2Inventory(randomizer.CurrentSettings, randomizer.Data.DoorDictionary);
        }
    }
}
