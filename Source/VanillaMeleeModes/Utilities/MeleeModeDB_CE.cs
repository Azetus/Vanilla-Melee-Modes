using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Utilities
{
    // ------------ Combat Extended ------------
    public static class MeleeModeDB_CE
    {
        public static VanillaMeleeModesModSetting Settings => VanillaMeleeModes.settings;


        // 近战命中率
        public static float GetMeleeHitChance_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_MeleeHitChance,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_MeleeHitChance,
                VMM_MeleeMode.Guard => Settings.CE_guard_MeleeHitChance,
                _ => 1f
            };
        }

        // 近战闪避率
        public static float GetMeleeDodgeChance_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_MeleeDodgeChance,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_MeleeDodgeChance,
                VMM_MeleeMode.Guard => Settings.CE_guard_MeleeDodgeChance,
                _ => 1f
            };
        }

        // 近战伤害
        public static float GetMeleeDamageFactor_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_MeleeDamageFactor,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_MeleeDamageFactor,
                VMM_MeleeMode.Guard => Settings.CE_guard_MeleeDamageFactor,
                _ => 1f
            };
        }

        // 近战冷却
        public static float GetMeleeCooldownFactor_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_MeleeCooldownFactor,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_MeleeCooldownFactor,
                VMM_MeleeMode.Guard => Settings.CE_guard_MeleeCooldownFactor,
                _ => 1f
            };
        }

        // 近战穿甲
        public static float GetMeleeArmorPenetration_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_ArmorPenetration,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_ArmorPenetration,
                VMM_MeleeMode.Guard => Settings.CE_guard_ArmorPenetration,
                _ => 1f
            };
        }

        // 近战格挡率
        public static float GetMeleeParryChanceFactor_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_MeleeParryChance,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_MeleeParryChance,
                VMM_MeleeMode.Guard => Settings.CE_guard_MeleeParryChance,
                _ => 1f
            };
        }

        // 近战暴击率
        public static float GetMeleeCritChanceFactor_CE(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.CE_aggressive_MeleeCritChance,
                VMM_MeleeMode.Flurry => Settings.CE_flurry_MeleeCritChance,
                VMM_MeleeMode.Guard => Settings.CE_guard_MeleeCritChance,
                _ => 1f
            };
        }
    }
}