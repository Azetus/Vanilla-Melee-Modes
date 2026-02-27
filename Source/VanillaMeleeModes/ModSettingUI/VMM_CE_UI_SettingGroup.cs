using UnityEngine;
using Verse;
using static VMM_VanillaMeleeModes.ModSettingUI.VMM_UI_SliderWithInput;

namespace VMM_VanillaMeleeModes.ModSettingUI
{
    internal static class VMM_CE_UI_SettingGroup
    {
        private const float Padding = 10f;
        private const float BottomSpacing = 15f;

        public static void CE_DrawSettingGroup(
            Listing_Standard ls,
            string title,
            ref float CE_meleeHitChance,
            ref float CE_meleeDodgeChance,
            ref float CE_meleeDamageFactor,
            ref float CE_meleeCooldownFactor,
            ref float CE_armorPenetration,
            ref float CE_meleeParryChance,
            ref float CE_meleeCritChance
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
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeHitChance_Label".Translate(), ref CE_meleeHitChance);
            // 近战闪避率
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeDodgeChance_Label".Translate(), ref CE_meleeDodgeChance);
            // 近战伤害乘数
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeDamageFactor_Label".Translate(), ref CE_meleeDamageFactor);
            // 近战冷却系数
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeCooldownFactor_Label".Translate(), ref CE_meleeCooldownFactor);
            // 近战穿甲系数
            DrawSliderWithInput_Float(innerLs, "VMM_ArmorPenetration_Label".Translate(), ref CE_armorPenetration);
            // 近战格挡
            DrawSliderWithInput_Float(innerLs, "VMM_MeleeParryChance_Label".Translate(), ref CE_meleeParryChance);
            // 近战暴击率
            DrawSliderWithInput_Float(innerLs, "VMM_CE_MeleeCritChance_Label".Translate(), ref CE_meleeCritChance);


            float contentHeight = innerLs.CurHeight;
            innerLs.End();

            // float finalHeight = contentHeight + Padding * 2f;
            // Widgets.DrawBox(new Rect(0f, startY, ls.ColumnWidth, finalHeight), 1);

            ls.Gap(contentHeight + BottomSpacing);
            ls.GapLine();
        }
    }
}