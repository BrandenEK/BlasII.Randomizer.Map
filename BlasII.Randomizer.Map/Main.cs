using MelonLoader;

namespace BlasII.Randomizer.Map
{
    public class Main : MelonMod
    {
        public static MapTracker MapTracker { get; private set; }

        public override void OnLateInitializeMelon()
        {
            MapTracker = new MapTracker();
        }
    }
}