using BlasII.Randomizer.Items;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    internal interface ILocation
    {
        public Image Image { get; set; }

        public Logic GetReachability(Blas2Inventory inventory);
    }
}
