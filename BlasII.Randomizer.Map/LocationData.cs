
namespace BlasII.Randomizer.Map
{
    internal class LocationData
    {
        public readonly float x;
        public readonly float y;
        public readonly string[] locations;

        public LocationData(float x, float y, string[] locations)
        {
            this.x = x;
            this.y = y;
            this.locations = locations;
        }
    }
}
