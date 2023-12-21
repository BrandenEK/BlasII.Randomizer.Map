﻿using BlasII.ModdingAPI.Input;
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
        private UIPixelTextWithShadow _nameText;

        private Sprite _locationImage;

        private Vector2 _lastCursor;
        private Vector2 _currentCursor;
        private int _selectedIndex = 0;

        /// <summary>
        /// Store the marker image
        /// </summary>
        public void LoadImage(Sprite image) => _locationImage = image;

        /// <summary>
        /// Recalculate the reachability for all locations
        /// </summary>
        public void Refresh(Blas2Inventory inventory)
        {
            // Create location holder and name text
            if (_locationHolder == null)
                CreateLocationHolder();
            if (_nameText == null)
                CreateNameText();

            // Update visibility of location holder
            _locationHolder.SetAsLastSibling();
            _locationHolder.gameObject.SetActive(Main.MapTracker.DisplayLocations);

            // Update logic status for all cells
            foreach (var location in Main.MapTracker.AllLocations.Values)
            {
                location.Image.color = Colors.LogicColors[location.GetReachability(inventory)];
            }

            // Clear text for selected location name
            _nameText.SetText(string.Empty);
        }

        /// <summary>
        /// Update the position of the location holder and content of the name text
        /// </summary>
        public void Update(Blas2Inventory inventory)
        {
            // Process changing cursor positions
            _currentCursor = CalculateCursorPosition();
            if (_currentCursor != _lastCursor)
                _selectedIndex = 0;
            _lastCursor = _currentCursor;

            // Process bumper input
            if (Main.MapTracker.InputHandler.GetButtonDown(ButtonType.UIBumperLeft))
                _selectedIndex--;
            if (Main.MapTracker.InputHandler.GetButtonDown(ButtonType.UIBumperRight))
                _selectedIndex++;

            // Process location holder and name text
            UpdateLocationHolder();
            UpdateNameText(inventory);

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

        private void UpdateLocationHolder()
        {
            if (_locationHolder == null)
                return;

            // Scroll position to the cell holder's position
            _locationHolder.position = _cellHolder.position;
        }

        private void UpdateNameText(Blas2Inventory inventory)
        {
            if (_nameText == null)
                return;

            // Ensure that the cursor is over a location
            if (!Main.MapTracker.AllLocations.TryGetValue(_currentCursor, out var location))
            {
                _nameText.SetText(string.Empty);
                return;
            }

            // Set text and color based on hovered location
            _nameText.SetText(location.GetNameAtIndex(_selectedIndex));
            _nameText.SetColor(Colors.LogicColors[location.GetReachabilityAtIndex(_selectedIndex, inventory)]);
        }

        /// <summary>
        /// Create the UI for all of the squares
        /// </summary>
        private void CreateLocationHolder()
        {
            var parent = Object.FindObjectOfType<MapWindowLogic>()?.mapContent;
            if (parent == null) return;

            // Create rect for ui holder
            Main.MapTracker.Log("Creating new location holder");
            _locationHolder = UIModder.CreateRect("LocationHolder", parent);
            _cellHolder = parent.GetChild(0).GetChild(0);

            // Create image for each item location
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

        /// <summary>
        /// Create the UI for the selected location name
        /// </summary>
        private void CreateNameText()
        {
            var parent = Object.FindObjectOfType<MapWindowLogic>()?.marksList?.transform;
            if (parent == null) return;

            // Remove mark ui
            if (parent.GetChild(0))
                Object.Destroy(parent.GetChild(0).gameObject);

            // Create text for location name
            Main.MapTracker.Log("Creating new name text");
            _nameText = UIModder.CreateRect("LocationName", parent).AddText().SetFontSize(50).AddShadow();
        }

        private Vector2 CalculateCursorPosition()
        {
            int x = (int)(_locationHolder.localPosition.x / -48 + 0.5f);
            int y = (int)(_locationHolder.localPosition.y / -48 + 0.5f);
            return new Vector2(x, y);
        }
    }
}