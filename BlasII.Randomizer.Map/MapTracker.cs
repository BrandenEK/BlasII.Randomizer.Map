using BlasII.ModdingAPI;
using BlasII.ModdingAPI.UI;
using BlasII.Randomizer.Map.Locations;
using Il2CppTGK.Game.Components.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    public class MapTracker : BlasIIMod
    {
        private Transform _locationHolder;
        private Transform _cellHolder;

        private Sprite _locationImage;
        private readonly Dictionary<Vector2, ILocation> _locationData = new();
        private readonly InventoryHandler _inventory = new();

        public bool IsMapOpen { get; private set; } = false;
        public bool DisplayLocations { get; private set; } = true;

        public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnInitialize()
        {
            FileHandler.LoadDataAsSprite("marker.png", out _locationImage, 10, true);
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

        public void OnOpenMap()
        {
            IsMapOpen = true;
            RefreshMap();
        }

        public void OnCloseMap()
        {
            IsMapOpen = false;
        }

        public void RefreshMap()
        {
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

            // Update visibility of location holder
            _locationHolder.SetAsLastSibling();
            _locationHolder.gameObject.SetActive(DisplayLocations);

            // Update logic status for all cells
            foreach (var location in _locationData.Values)
            {
                location.Image.color = Colors.LogicColors[location.GetReachability(_inventory.CurrentInventory)];
            }
        }

        public void UpdateMap()
        {
            if (_locationHolder == null)
                return;

            _locationHolder.position = _cellHolder.position;

            if (InputHandler.GetKeyDown("ToggleLocations"))
            {
                DisplayLocations = !DisplayLocations;
                RefreshMap();
            }

            // Only do this next part if debug

            Transform location = _locationHolder.GetChild(_locationHolder.childCount - 1);
            var movement = new Vector3();

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                movement.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                movement.y = -1;
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                movement.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                movement.x = 1;
            }

            location.localPosition += movement * 48 * (Input.GetKey(KeyCode.Keypad0) ? 3 : 1);

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
                Log($"x: {location.localPosition.x / 48}, y: {location.localPosition.y / 48}");
        }

        private void CreateLocationHolder()
        {
            var parent = Object.FindObjectOfType<MapWindowLogic>()?.mapContent;
            if (parent == null)
                return;

            Log("Creating new location holder");
            _locationHolder = UIModder.CreateRect("LocationHolder", parent);
            _cellHolder = parent.GetChild(0).GetChild(0);

            foreach (var location in _locationData)
            {
                var rect = UIModder.CreateRect($"Location {location.Key}", _locationHolder);
                rect.localPosition = location.Key * 48;
                rect.sizeDelta = new Vector2(30, 30);

                var image = rect.gameObject.AddComponent<Image>();
                image.sprite = _locationImage;
                image.color = Color.red;

                location.Value.Image = image;
            }
        }
    }
}
