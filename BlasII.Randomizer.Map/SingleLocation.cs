using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    internal class SingleLocation : ILocation
    {
        private readonly string _id;

        // Stored when the map UI is created
        public Image Image { get; set; }

        public SingleLocation(string id) => _id = id;
    }
}
