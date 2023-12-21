using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    internal class MultipleLocation : ILocation
    {
        private readonly string[] _ids;

        // Stored when the map UI is created
        public Image Image { get; set; }

        public MultipleLocation(string[] ids) => _ids = ids;
    }
}
