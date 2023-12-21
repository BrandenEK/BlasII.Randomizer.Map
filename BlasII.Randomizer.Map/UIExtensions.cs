using BlasII.ModdingAPI.UI;
using Il2CppTGK.Game.Components.UI;
using Il2CppTMPro;
using UnityEngine;

namespace BlasII.Randomizer.Map
{
    internal static class UIExtensions
    {
        public static UIPixelTextWithShadow AddShadow(this TextMeshProUGUI text)
        {
            // Create overhead text object
            RectTransform newRect = Object.Instantiate(text.gameObject).GetComponent<RectTransform>();
            newRect.SetParent(text.transform, false);

            // Change positions
            newRect.ChangePosition(0, 4);
            text.rectTransform.ChangePosition(0, -2);

            // Change the underneath text object's color to shadow
            TextMeshProUGUI newText = newRect.GetComponent<TextMeshProUGUI>();
            newText.color = text.color;
            text.SetColor(new Color(0.004f, 0.008f, 0.008f));

            // Add pixel text component
            UIPixelTextWithShadow shadow = text.gameObject.AddComponent<UIPixelTextWithShadow>();
            shadow.normalText = newText;
            shadow.shadowText = text;
            return shadow;
        }

        public static RectTransform ChangePosition(this RectTransform rect, Vector2 position)
        {
            rect.anchoredPosition += position;
            return rect;
        }

        public static RectTransform ChangePosition(this RectTransform rect, float x, float y) => rect.ChangePosition(new Vector2(x, y));
    }
}
