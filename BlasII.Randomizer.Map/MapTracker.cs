using BlasII.ModdingAPI;
using Il2CppInterop.Runtime;
using Il2CppTGK.Game.Components.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Map
{
    public class MapTracker : BlasIIMod
    {
        private Transform _locationHolder;
        private Transform _cellHolder;

        private Sprite _locationImage;

        public MapTracker() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

        protected override void OnInitialize()
        {
            FileHandler.LoadDataAsSprite("marker.png", out _locationImage, 10, true);
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

            // Update logic status for all cells
        }

        public void UpdateMap()
        {
            if (_locationHolder == null)
                return;

            _locationHolder.position = _cellHolder.position;

            // Only do this next part if debug

            Transform location = _locationHolder.GetChild(_locationHolder.childCount - 1);
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                location.localPosition = new Vector3(location.localPosition.x, location.localPosition.y + 48);
                Log(location.localPosition / 48);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                location.localPosition = new Vector3(location.localPosition.x, location.localPosition.y - 48);
                Log(location.localPosition / 48);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                location.localPosition = new Vector3(location.localPosition.x - 48, location.localPosition.y);
                Log(location.localPosition / 48);
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                location.localPosition = new Vector3(location.localPosition.x + 48, location.localPosition.y);
                Log(location.localPosition / 48);
            }
        }

        private void CreateLocationHolder()
        {
            var parent = Object.FindObjectOfType<MapWindowLogic>()?.mapContent;
            if (parent == null)
                return;

            Log("Creating new location holder");
            _locationHolder = CreateRect(parent, "LocationHolder");
            _cellHolder = parent.GetChild(0).GetChild(0);

            foreach (var location in Data.MapLocations)
            {
                var rect = CreateRect(_locationHolder, "locId");
                rect.localPosition = location.Key * 48;
                rect.sizeDelta = new Vector2(30, 30);

                var image = rect.gameObject.AddComponent<Image>();
                image.sprite = _locationImage;
                image.color = Color.red;
            }
        }

        private RectTransform CreateRect(Transform parent, string name)
        {
            var obj = new GameObject(name, Il2CppType.From(typeof(RectTransform)));
            var rect = obj.GetComponent<RectTransform>();
            rect.SetParent(parent, false);
            return rect;
        }
    }
}
