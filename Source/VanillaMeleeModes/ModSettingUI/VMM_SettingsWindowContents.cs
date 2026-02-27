using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VMM_VanillaMeleeModes.Settings;
using static VMM_VanillaMeleeModes.ModSettingUI.VMM_UI_SettingGroup;
using static VMM_VanillaMeleeModes.ModSettingUI.VMM_CE_UI_SettingGroup;

namespace VMM_VanillaMeleeModes.ModSettingUI
{
    internal static class VMM_SettingsWindowContents
    {
        private static Vector2 scrollPos;
        private static float lastCalculatedHeight = 1000f;

        public static void SettingsWindowContents(Rect inRect, ref VanillaMeleeModesModSetting settings)
        {
            const float footerHeight = 45f;

            // 滚动条区域 开始
            Rect scrollOutRect = new Rect(inRect.x, inRect.y + 10f, inRect.width, inRect.height - footerHeight - 15f);
            Rect viewRect = new Rect(0f, 0f, inRect.width - 24f, lastCalculatedHeight);
            Widgets.BeginScrollView(scrollOutRect, ref scrollPos, viewRect);
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(viewRect);


            if (!VanillaMeleeModes.isCEActive)
            {
                // 强攻
                DrawSettingGroup(
                    ls,
                    "VMM_AggressiveMode".Translate(),
                    ref settings.aggressive_MeleeHitChance,
                    ref settings.aggressive_MeleeDodgeChance,
                    ref settings.aggressive_MeleeDamageFactor,
                    ref settings.aggressive_MeleeCooldownFactor,
                    ref settings.aggressive_ArmorPenetration,
                    ref settings.aggressive_MeleeParryChance,
                    ref settings.aggressive_MeleeParryDamageReduction,
                    ref settings.aggressive_MeleeCounterChance
                );
                // 迅捷
                DrawSettingGroup(
                    ls,
                    "VMM_FlurryMode".Translate(),
                    ref settings.flurry_MeleeHitChance,
                    ref settings.flurry_MeleeDodgeChance,
                    ref settings.flurry_MeleeDamageFactor,
                    ref settings.flurry_MeleeCooldownFactor,
                    ref settings.flurry_ArmorPenetration,
                    ref settings.flurry_MeleeParryChance,
                    ref settings.flurry_MeleeParryDamageReduction,
                    ref settings.flurry_MeleeCounterChance
                );
                // 防守
                DrawSettingGroup(
                    ls,
                    "VMM_GuardMode".Translate(),
                    ref settings.guard_MeleeHitChance,
                    ref settings.guard_MeleeDodgeChance,
                    ref settings.guard_MeleeDamageFactor,
                    ref settings.guard_MeleeCooldownFactor,
                    ref settings.guard_ArmorPenetration,
                    ref settings.guard_MeleeParryChance,
                    ref settings.guard_MeleeParryDamageReduction,
                    ref settings.guard_MeleeCounterChance
                );
            }

            if (VanillaMeleeModes.isCEActive)
            {
                ls.Label("VMM_CE_IsActive_Label".Translate());
                // ------ 强攻 (Aggressive) ------
                CE_DrawSettingGroup(
                    ls,
                    "VMM_AggressiveMode".Translate(),
                    ref settings.CE_aggressive_MeleeHitChance,
                    ref settings.CE_aggressive_MeleeDodgeChance,
                    ref settings.CE_aggressive_MeleeDamageFactor,
                    ref settings.CE_aggressive_MeleeCooldownFactor,
                    ref settings.CE_aggressive_ArmorPenetration,
                    ref settings.CE_aggressive_MeleeParryChance,
                    ref settings.CE_aggressive_MeleeCritChance
                );

                // ------ 迅捷 (Flurry) ------
                CE_DrawSettingGroup(
                    ls,
                    "VMM_FlurryMode".Translate(),
                    ref settings.CE_flurry_MeleeHitChance,
                    ref settings.CE_flurry_MeleeDodgeChance,
                    ref settings.CE_flurry_MeleeDamageFactor,
                    ref settings.CE_flurry_MeleeCooldownFactor,
                    ref settings.CE_flurry_ArmorPenetration,
                    ref settings.CE_flurry_MeleeParryChance,
                    ref settings.CE_flurry_MeleeCritChance
                );

                // ------ 防御 (Guard) ------
                CE_DrawSettingGroup(
                    ls,
                    "VMM_GuardMode".Translate(),
                    ref settings.CE_guard_MeleeHitChance,
                    ref settings.CE_guard_MeleeDodgeChance,
                    ref settings.CE_guard_MeleeDamageFactor,
                    ref settings.CE_guard_MeleeCooldownFactor,
                    ref settings.CE_guard_ArmorPenetration,
                    ref settings.CE_guard_MeleeParryChance,
                    ref settings.CE_guard_MeleeCritChance
                );
            }

            lastCalculatedHeight = ls.CurHeight + 20f;
            ls.End();
            Widgets.EndScrollView();
            // 滚动条区域 结束
            ls.GapLine();


            Rect footerRect = new Rect(inRect.x, inRect.yMax - footerHeight + 5f, inRect.width, footerHeight);

            GUI.color = new Color(1f, 1f, 1f, 0.3f);
            Widgets.DrawLineHorizontal(footerRect.x, footerRect.y, footerRect.width);
            GUI.color = Color.white;
            // Reset button
            Rect resetRect = new Rect(footerRect.xMax - 240f, footerRect.y + 5f, 240f, 30f);
            ;
            if (Widgets.ButtonText(resetRect, "VMM_ResetButton_Label".Translate()))
            {
                if (!VanillaMeleeModes.isCEActive)
                {
                    settings.ResetSetting();
                }

                if (VanillaMeleeModes.isCEActive)
                {
                    settings.ResetSetting_CE();
                }

                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            GUI.color = Color.white;
        }
    }
}