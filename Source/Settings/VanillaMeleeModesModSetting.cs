using Verse;

namespace VMM_VanillaMeleeModes.Settings
{
    public class VanillaMeleeModesModSetting : ModSettings
    {
        // ------ 强攻 (Aggressive) ------
        public float aggressive_MeleeHitChance = 1f;
        public float aggressive_MeleeDodgeChance = 1f;
        public float aggressive_MeleeDamageFactor = 1f;
        public float aggressive_MeleeCooldownFactor = 1f;

        // ------ 迅捷 (Flurry) ------
        public float flurry_MeleeHitChance = 1f;
        public float flurry_MeleeDodgeChance = 1f;
        public float flurry_MeleeDamageFactor = 1f;
        public float flurry_MeleeCooldownFactor = 1f;

        // ------ 防御 (Guard) ------
        public float guard_MeleeHitChance = 1f;
        public float guard_MeleeDodgeChance = 1f;
        public float guard_MeleeDamageFactor = 1f;
        public float guard_MeleeCooldownFactor = 1f;

        public override void ExposeData()
        {
            // ------ 强攻 (Aggressive) ------
            Scribe_Values.Look(ref aggressive_MeleeHitChance, "aggressive_MeleeHitChance", 1f);
            Scribe_Values.Look(ref aggressive_MeleeDodgeChance, "aggressive_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref aggressive_MeleeDamageFactor, "aggressive_MeleeDamageFactor", 1f);
            Scribe_Values.Look(ref aggressive_MeleeCooldownFactor, "aggressive_MeleeCooldownFactor", 1f);

            // ------ 迅捷 (Flurry) ------
            Scribe_Values.Look(ref flurry_MeleeHitChance, "flurry_MeleeHitChance", 1f);
            Scribe_Values.Look(ref flurry_MeleeDodgeChance, "flurry_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref flurry_MeleeDamageFactor, "flurry_MeleeDamageFactor", 1f);
            Scribe_Values.Look(ref flurry_MeleeCooldownFactor, "flurry_MeleeCooldownFactor", 1f);

            // ------ 防御 (Guard) ------
            Scribe_Values.Look(ref guard_MeleeHitChance, "guard_MeleeHitChance", 1f);
            Scribe_Values.Look(ref guard_MeleeDodgeChance, "guard_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref guard_MeleeDamageFactor, "guard_MeleeDamageFactor", 1f);
            Scribe_Values.Look(ref guard_MeleeCooldownFactor, "guard_MeleeCooldownFactor", 1f);


            base.ExposeData();
        }

        public void ResetSetting()
        {
            // ------ 强攻 (Aggressive) ------
            aggressive_MeleeHitChance = 1f;
            aggressive_MeleeDodgeChance = 1f;
            aggressive_MeleeDamageFactor = 1f;
            aggressive_MeleeCooldownFactor = 1f;

            // ------ 迅捷 (Flurry) ------
            flurry_MeleeHitChance = 1f;
            flurry_MeleeDodgeChance = 1f;
            flurry_MeleeDamageFactor = 1f;
            flurry_MeleeCooldownFactor = 1f;

            // ------ 防御 (Guard) ------
            guard_MeleeHitChance = 1f;
            guard_MeleeDodgeChance = 1f;
            guard_MeleeDamageFactor = 1f;
            guard_MeleeCooldownFactor = 1f;
        }
    }
}
