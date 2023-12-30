using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Map;
using UnityEngine;

namespace BlasII.Randomizer.Map
{
    /// <summary>
    /// Process opening and closing the map
    /// </summary>
    //[HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.OnShow))]
    //class Map_Show_Patch
    //{
    //    public static void Postfix(MapWindowLogic __instance)
    //    {
    //        //Main.MapTracker.LogWarning("Mode: " + __instance.currentMapMode);
    //        //NormalMapMode = __instance.currentMapMode == MapWindowLogic.MapMode.Normal;
    //        Main.MapTracker.LogWarning("Before awake");

    //        //var cellHolder = __instance.mapContent?.GetChild(0)?.GetChild(0)?.GetChild(0);
    //        //for (int i = 0; i < cellHolder.childCount; i++)
    //        //{
    //        //    Main.MapTracker.LogError("Deleting: " + cellHolder.GetChild(i).name);
    //        //    Object.Destroy(cellHolder.GetChild(i).gameObject);

    //        //}

    //        //var allCells = CoreCache.Map.GetAllCells().ToArray();
    //        //var revealedCells = CoreCache.Map.GetRevealedCells().ToArray();

    //        //foreach (var cell in allCells)
    //        //    __instance.uiRenderNormal.HideCell(cell);
    //        //foreach (var cell in Main.MapTracker.DisplayLocations ? allCells : revealedCells)
    //        //    __instance.uiRenderNormal.ShowCell(cell);

    //        //__instance.OnAwake();//__instance.SetMapMode(MapWindowLogic.MapMode.Normal);
    //        Main.MapTracker.LogWarning("After awake");
    //        //Main.MapTracker.OnOpenMap();
    //}

    //    public static bool NormalMapMode { get; set; }
    //}
    [HarmonyPatch(typeof(MapWindowLogic), nameof(MapWindowLogic.SetMapMode))]
    class Map_Show_Patch
    {
        public static void Postfix(MapWindowLogic.MapMode mode) =>
            Main.MapTracker.OnOpenMap(mode == MapWindowLogic.MapMode.Normal);
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
    //[HarmonyPatch(typeof(MapManager), nameof(MapManager.GetRevealedCells))]
    //class Map_Reveal_Patch
    //{
    //    public static bool Prefix(MapManager __instance, ref IEnumerable<CellData> __result)
    //    {
    //        //return true;
    //        //Main.MapTracker.LogError("Checking all revealed cells");
    //        //if (!Main.MapTracker.DisplayLocations)
    //        //    return true;

    //        //var cellsToShow = new List<CellData>();

    //        //foreach (var cell in __instance.GetAllCells().ToArray())
    //        //{
    //        //    //Main.MapTracker.Log($"({cell.key.x}, {cell.key.y}): {cell.type.name}");
    //        //    if (cell.type.name.ToLower() == "priedieu")
    //        //    {
    //        //        //Main.MapTracker.Log($"Skipping cell: {cell.key.x} {cell.key.y}");
    //        //        continue;
    //        //    }

    //        //    cellsToShow.Add(cell);
    //        //}

    //        __result = __instance.GetAllCells();
    //        return false;
    //    }
    //}
}
