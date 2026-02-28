using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Utilities
{
    // ------------ Vanilla ------------
    public static class MeleeModeDB
    {
        public static VanillaMeleeModesModSetting Settings => VanillaMeleeModes.settings;

        // 近战命中率
        public static float GetMeleeHitChance(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeHitChance,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeHitChance,
                VMM_MeleeMode.Guard => Settings.guard_MeleeHitChance,
                _ => 1f

            };
        }

        // 近战闪避率
        public static float GetMeleeDodgeChance(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeDodgeChance,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeDodgeChance,
                VMM_MeleeMode.Guard => Settings.guard_MeleeDodgeChance,
                _ => 1f

            };
        }

        // 近战伤害
        public static float GetMeleeDamageFactor(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeDamageFactor,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeDamageFactor,
                VMM_MeleeMode.Guard => Settings.guard_MeleeDamageFactor,
                _ => 1f

            };
        }

        // 近战冷却
        public static float GetMeleeCooldownFactor(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeCooldownFactor,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeCooldownFactor,
                VMM_MeleeMode.Guard => Settings.guard_MeleeCooldownFactor,
                _ => 1f
            };
        }

        // 近战穿甲
        public static float GetMeleeArmorPenetration(VMM_MeleeMode mode) {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_ArmorPenetration,
                VMM_MeleeMode.Flurry => Settings.flurry_ArmorPenetration,
                VMM_MeleeMode.Guard => Settings.guard_ArmorPenetration,
                _ => 1f
            };
        }

        // 近战格挡率
        public static float GetMeleeParryChanceFactor(VMM_MeleeMode mode) {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeParryChance,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeParryChance,
                VMM_MeleeMode.Guard => Settings.guard_MeleeParryChance,
                _ => 1f
            };
        }

        // 近战格挡减伤
        public static float GetMeleeParryDamageReductionFactor(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeParryDamageReduction,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeParryDamageReduction,
                VMM_MeleeMode.Guard => Settings.guard_MeleeParryDamageReduction,
                _ => 1f
            };
        }

        // 近战反击率
        public static float GetMeleeCounterChanceFactor(VMM_MeleeMode mode)
        {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_MeleeCounterChance,
                VMM_MeleeMode.Flurry => Settings.flurry_MeleeCounterChance,
                VMM_MeleeMode.Guard => Settings.guard_MeleeCounterChance,
                _ => 1f
            };
        }
    }
}
