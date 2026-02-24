using Verse;

namespace VMM_VanillaMeleeModes.Settings
{
    public class VanillaMeleeModesModSetting : ModSettings
    {
        // ------ 强攻 (Aggressive) ------
        public float aggressive_MeleeHitChance = 1.15f;
        public float aggressive_MeleeDodgeChance = 0.85f;
        public float aggressive_MeleeDamageFactor = 1.25f;
        public float aggressive_MeleeCooldownFactor = 1.35f;
        public float aggressive_ArmorPenetration = 1.10f;

        // ------ 迅捷 (Flurry) ------
        public float flurry_MeleeHitChance = 0.95f;
        public float flurry_MeleeDodgeChance = 1f;
        public float flurry_MeleeDamageFactor = 0.8f;
        public float flurry_MeleeCooldownFactor = 0.75f;
        public float flurry_ArmorPenetration = 0.9f;

        // ------ 防御 (Guard) ------
        public float guard_MeleeHitChance = 0.9f;
        public float guard_MeleeDodgeChance = 1.5f;
        public float guard_MeleeDamageFactor = 0.75f;
        public float guard_MeleeCooldownFactor = 1.05f;
        public float guard_ArmorPenetration = 0.85f;

        public override void ExposeData()
        {
            // ------ 强攻 (Aggressive) ------
            Scribe_Values.Look(ref aggressive_MeleeHitChance, "aggressive_MeleeHitChance", 1.15f);
            Scribe_Values.Look(ref aggressive_MeleeDodgeChance, "aggressive_MeleeDodgeChance", 0.85f);
            Scribe_Values.Look(ref aggressive_MeleeDamageFactor, "aggressive_MeleeDamageFactor", 1.25f);
            Scribe_Values.Look(ref aggressive_MeleeCooldownFactor, "aggressive_MeleeCooldownFactor", 1.35f);
            Scribe_Values.Look(ref aggressive_ArmorPenetration, "aggressive_ArmorPenetration", 1.10f);

            // ------ 迅捷 (Flurry) ------
            Scribe_Values.Look(ref flurry_MeleeHitChance, "flurry_MeleeHitChance", 0.95f);
            Scribe_Values.Look(ref flurry_MeleeDodgeChance, "flurry_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref flurry_MeleeDamageFactor, "flurry_MeleeDamageFactor", 0.8f);
            Scribe_Values.Look(ref flurry_MeleeCooldownFactor, "flurry_MeleeCooldownFactor", 0.75f);
            Scribe_Values.Look(ref flurry_ArmorPenetration, "flurry_ArmorPenetration", 0.9f);

            // ------ 防御 (Guard) ------
            Scribe_Values.Look(ref guard_MeleeHitChance, "guard_MeleeHitChance", 0.9f);
            Scribe_Values.Look(ref guard_MeleeDodgeChance, "guard_MeleeDodgeChance", 1.5f);
            Scribe_Values.Look(ref guard_MeleeDamageFactor, "guard_MeleeDamageFactor", 0.75f);
            Scribe_Values.Look(ref guard_MeleeCooldownFactor, "guard_MeleeCooldownFactor", 1.05f);
            Scribe_Values.Look(ref guard_ArmorPenetration, "guard_ArmorPenetration", 0.85f);


            base.ExposeData();
        }

        public void ResetSetting()
        {
            // ------ 强攻 (Aggressive) ------
            aggressive_MeleeHitChance = 1.15f;
            aggressive_MeleeDodgeChance = 0.85f;
            aggressive_MeleeDamageFactor = 1.25f;
            aggressive_MeleeCooldownFactor = 1.35f;
            aggressive_ArmorPenetration = 1.10f;

            // ------ 迅捷 (Flurry) ------
            flurry_MeleeHitChance = 0.95f;
            flurry_MeleeDodgeChance = 1f;
            flurry_MeleeDamageFactor = 0.8f;
            flurry_MeleeCooldownFactor = 0.75f;
            flurry_ArmorPenetration = 0.9f;

            // ------ 防御 (Guard) ------
            guard_MeleeHitChance = 0.9f;
            guard_MeleeDodgeChance = 1.5f;
            guard_MeleeDamageFactor = 0.75f;
            guard_MeleeCooldownFactor = 1.05f;
            guard_ArmorPenetration = 0.85f;
        }
    }
}
