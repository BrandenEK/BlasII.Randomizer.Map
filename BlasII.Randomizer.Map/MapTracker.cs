using BlasII.ModdingAPI;
using BlasII.ModdingAPI.UI;
using Il2CppInterop.Runtime;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    public class MapTracker : BlasIIMod
    {
        private Transform _locationHolder;

        public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnInitialize()
        {
            LogError($"{ModInfo.MOD_NAME} is initialized");
        }

        public void RefreshMap()
        {
            var map = Object.FindObjectOfType<MapWindowLogic>();
            LogWarning(map.transform.DisplayHierarchy(10, true));

            // Create location holder and move to top
            if (_locationHolder == null)
            {
                CreateLocationHolder();
                if (_locationHolder == null)
                {
                    LogError("Failed to create map location holder!");
                    return;
                }
            }
            _locationHolder.SetAsLastSibling();

        }

        private void CreateLocationHolder()
        {
            var parent = Object.FindObjectOfType<MapWindowLogic>()?.transform.Find("Background/Map/MapMask/RootMap")?.GetChild(0);
            if (parent == null)
                return;

            Log("Creating new location holder");
            _locationHolder = new GameObject("LocationHolder", Il2CppType.From(typeof(RectTransform))).transform;
            _locationHolder.SetParent(parent, false);

            //_locationHolder.gameObject.AddComponent<Image>();
        }
    }
}
