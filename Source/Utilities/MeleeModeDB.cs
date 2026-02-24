using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Utilities
{
    public static class MeleeModeDB
    {
        public static VanillaMeleeModesModSetting Settings => VanillaMeleeModes.settings;

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

        public static float GetMeleeArmorPenetration(VMM_MeleeMode mode) {
            return mode switch
            {
                VMM_MeleeMode.Aggressive => Settings.aggressive_ArmorPenetration,
                VMM_MeleeMode.Flurry => Settings.flurry_ArmorPenetration,
                VMM_MeleeMode.Guard => Settings.guard_ArmorPenetration,
                _ => 1f
            };
        }

    }
}
