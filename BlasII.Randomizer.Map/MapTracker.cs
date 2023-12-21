using BlasII.ModdingAPI;
using BlasII.Randomizer.Map.Locations;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Randomizer.Map
{
    public class MapTracker : BlasIIMod
    {
        private readonly InventoryHandler _inventory = new();
        private readonly UIHandler _ui = new();

        private readonly Dictionary<Vector2, ILocation> _locationData = new();
        internal Dictionary<Vector2, ILocation> AllLocations => _locationData;

        public bool IsMapOpen { get; private set; } = false;
        public bool DisplayLocations { get; private set; } = true;

        public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnInitialize()
        {
            if (FileHandler.LoadDataAsSprite("marker.png", out Sprite image, 10, true))
            {
                _ui.LoadImage(image);
            }
            InputHandler.RegisterDefaultKeybindings(new Dictionary<string, KeyCode>()
            {
                { "ToggleLocations", KeyCode.F7 }
            });
            MessageHandler.AllowReceivingBroadcasts = true;
            MessageHandler.AddMessageListener("BlasII.Randomizer", "LOCATION", (content) =>
            {
                _inventory.Refresh();
            });

            LoadLocationData();
        }

        private void LoadLocationData()
        {
            if (!FileHandler.LoadDataAsJson("locations.json", out LocationData[] locations))
            {
                LogError("Failed to load location data!");
                return;
            }

            foreach (var data in locations)
            {
                if (data.locations == null || data.locations.Length == 0)
                    continue;

                _locationData.Add(new Vector2(data.x, data.y), data.locations.Length == 1
                    ? new SingleLocation(data.locations[0])
                    : new MultipleLocation(data.locations));
            }
        }

        protected override void OnSceneLoaded(string sceneName)
        {
            if (sceneName == "MainMenu")
                _inventory.Refresh();
        }

        protected override void OnUpdate()
        {
            if (!IsMapOpen) return;

            if (InputHandler.GetKeyDown("ToggleLocations"))
            {
                DisplayLocations = !DisplayLocations;
                _ui.Refresh(_inventory.CurrentInventory);
            }

            _ui.Update();
        }

        public void OnOpenMap()
        {
            IsMapOpen = true;
            _ui.Refresh(_inventory.CurrentInventory);
        }

        public void OnCloseMap()
        {
            IsMapOpen = false;
        }
    }
}
