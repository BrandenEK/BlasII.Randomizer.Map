using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Map;

namespace BlasII.Randomizer.Map
{
    /// <summary>
    /// Process opening and closing the map
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnShow))]
    class Map_Show_Patch
    {
        public static void Postfix() => Main.MapTracker.OnOpenMap();
    }
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnClose))]
    class Map_Close_Patch
    {
        public static void Postfix() => Main.MapTracker.OnCloseMap();
    }

    /// <summary>
    /// Toggle location display when zooming the map
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.ZoomIn))]
    class Map_ZoomIn_Patch
    {
        public static void Postfix() => Main.MapTracker.OnZoomIn();
    }
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.ZoomOut))]
    class Map_ZoomOut_Patch
    {
        public static void Postfix() => Main.MapTracker.OnZoomOut();
    }

    /// <summary>
    /// Always prevent placing marks
    /// </summary>
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.CanPlaceMarker))]
    class Map_Marker_Patch
    {
        public static bool Prefix(ref bool __result) => __result = false;
    }

    /// <summary>
    /// Reveal the entire map if locations are showing
    /// </summary>
    [HarmonyPatch(typeof(MapManager), nameof(MapManager.GetRevealedCells))]
    class Map_Reveal_Patch
    {
        public static bool Prefix(MapManager __instance, ref IEnumerable<CellData> __result)
        {
            //if (!Main.MapTracker.DisplayLocations)
            //    return true;

            __result = __instance.GetAllCells();
            return false;
        }
    }
}
