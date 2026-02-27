using UnityEngine;
using Verse;

namespace VMM_VanillaMeleeModes.ModSettingUI
{
    internal static class VMM_UI_SliderWithInput
    {
        public static void DrawSliderWithInput_Float(
           Listing_Standard ls,
           string label,
           ref float value,
           float min = 0.1f,
           float max = 3f)
        {
            const float labelWidth = 120f;
            const float min_labelHeight = 30f;
            const float fieldWidth = 60f;
            const float gap = 10f;

            float labelHeight = Text.CalcHeight(label, labelWidth);
            float rowHeight = Math.Max(labelHeight, min_labelHeight);
            Rect row = ls.GetRect(rowHeight);

            Rect labelRect = new Rect(row.x, row.y, labelWidth, rowHeight);
            Rect sliderRect = new Rect(labelRect.xMax + gap, row.y, row.width - labelWidth - fieldWidth - gap * 2, rowHeight);
            Rect fieldRect = new Rect(sliderRect.xMax + gap, row.y, fieldWidth, min_labelHeight);

            Color oldColor = GUI.color;
            GUI.color = Color.white;
            Widgets.Label(labelRect, label);
            GUI.color = oldColor;

            value = Widgets.HorizontalSlider(
                sliderRect,
                value,
                min,
                max,
                true,
                value.ToString("0.00"));

            string buffer = value.ToString("0.00");

            Widgets.TextFieldNumeric(
                fieldRect,
                ref value,
                ref buffer,
                min,
                max);
        }
    }
}
