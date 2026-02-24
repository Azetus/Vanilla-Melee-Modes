using UnityEngine;
using Verse;
using static VMM_VanillaMeleeModes.ModSettingUI.VMM_UI_SliderWithInput;

namespace VMM_VanillaMeleeModes.ModSettingUI
{
    internal static class VMM_UI_SettingGroup
    {
        const float Padding = 10f;
        const float BottomSpacing = 15f;

        public static void DrawSettingGroup(
            Listing_Standard ls,
            string title,
            ref float meleeHitChance,
            ref float meleeDodgeChance,
            ref float meleeDamageFactor,
            ref float meleeCooldownFactor,
            ref float armorPenetration
            )
        {

            float startY = ls.CurHeight;

            Rect contentRect = new Rect(0f, startY, ls.ColumnWidth, 10000f);

            Listing_Standard innerLs = new Listing_Standard();

            innerLs.Begin(contentRect.ContractedBy(Padding));

            Text.Font = GameFont.Medium;
            innerLs.Label(title);
            Text.Font = GameFont.Small;
            innerLs.Gap(6f);

            // 近战命中率
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeHitChance_Label".Translate(), ref meleeHitChance);
            // 近战闪避率
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeDodgeChance_Label".Translate(), ref meleeDodgeChance);
            // 近战伤害乘数
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeDamageFactor_Label".Translate(), ref meleeDamageFactor);
            // 近战冷却系数
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeCooldownFactor_Label".Translate(), ref meleeCooldownFactor);
            // 近战穿甲系数
            DrawSliderWithInput_Float(innerLs, "VMM_ArmorPenetration_Label".Translate(), ref armorPenetration);

            float contentHeight = innerLs.CurHeight;
            innerLs.End();

            // float finalHeight = contentHeight + Padding * 2f;
            // Widgets.DrawBox(new Rect(0f, startY, ls.ColumnWidth, finalHeight), 1);
 
            ls.Gap(contentHeight + BottomSpacing);
            ls.GapLine();
        }
    }
}
