using HarmonyLib;
using Il2CppTGK.Game.Components.UI;

namespace BlasII.Randomizer.Map
{
    /// <summary>
    /// Refresh the map whenever it is opened
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnShow))]
    class Map_Show_Patch
    {
        public static void Postfix() => Main.MapTracker.RefreshMap();
    }

    /// <summary>
    /// Update the location positions whenever it is updated
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.Update))]
    class Map_Update_Patch
    {
        public static void Postfix() => Main.MapTracker.UpdateMap();
    }
}
