using BlasII.ModdingAPI.UI;
using BlasII.Randomizer.Items;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    internal class UIHandler
    {
        private Transform _locationHolder;
        private Transform _cellHolder;

        private Sprite _locationImage;

        /// <summary>
        /// Store the marker image
        /// </summary>
        public void LoadImage(Sprite image) => _locationImage = image;

        /// <summary>
        /// Recalculate the reachability for all locations
        /// </summary>
        public void Refresh(Blas2Inventory inventory)
        {
            // Create location holder and move to top
            if (_locationHolder == null)
            {
                CreateLocationHolder();
                if (_locationHolder == null)
                {
                    Main.MapTracker.LogError("Failed to create map location holder!");
                    return;
                }
            }

            // Update visibility of location holder
            _locationHolder.SetAsLastSibling();
            _locationHolder.gameObject.SetActive(Main.MapTracker.DisplayLocations);

            // Update logic status for all cells
            foreach (var location in Main.MapTracker.AllLocations.Values)
            {
                location.Image.color = Colors.LogicColors[location.GetReachability(inventory)];
            }
        }

        /// <summary>
        /// Update the position of the location holder
        /// </summary>
        public void Update()
        {
            if (_locationHolder == null)
                return;

            _locationHolder.position = _cellHolder.position;

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
                Main.MapTracker.Log($"x: {location.localPosition.x / 48}, y: {location.localPosition.y / 48}");
        }

        /// <summary>
        /// Create the UI for all of the squares
        /// </summary>
        private void CreateLocationHolder()
        {
            var parent = Object.FindObjectOfType<MapWindowLogic>()?.mapContent;
            if (parent == null)
                return;

            Main.MapTracker.Log("Creating new location holder");
            _locationHolder = UIModder.CreateRect("LocationHolder", parent);
            _cellHolder = parent.GetChild(0).GetChild(0);

            foreach (var location in Main.MapTracker.AllLocations)
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
