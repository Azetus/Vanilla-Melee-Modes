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

        public float aggressive_MeleeParryChance = 0.6f;
        public float aggressive_MeleeParryDamageReduction = 0.4f;
        public float aggressive_MeleeCounterChance = 0.75f;


        // ------ 迅捷 (Flurry) ------
        public float flurry_MeleeHitChance = 0.95f;
        public float flurry_MeleeDodgeChance = 1f;
        public float flurry_MeleeDamageFactor = 0.8f;
        public float flurry_MeleeCooldownFactor = 0.75f;
        public float flurry_ArmorPenetration = 0.9f;

        public float flurry_MeleeParryChance = 1.0f;
        public float flurry_MeleeParryDamageReduction = 0.5f;
        public float flurry_MeleeCounterChance = 0.5f;

        // ------ 防御 (Guard) ------
        public float guard_MeleeHitChance = 0.9f;
        public float guard_MeleeDodgeChance = 1.5f;
        public float guard_MeleeDamageFactor = 0.75f;
        public float guard_MeleeCooldownFactor = 1.05f;
        public float guard_ArmorPenetration = 0.85f;

        public float guard_MeleeParryChance = 1.6f;
        public float guard_MeleeParryDamageReduction = 0.75f;
        public float guard_MeleeCounterChance = 0.3f;

        public override void ExposeData()
        {
            // ------ 强攻 (Aggressive) ------
            Scribe_Values.Look(ref aggressive_MeleeHitChance, "aggressive_MeleeHitChance", 1.15f);
            Scribe_Values.Look(ref aggressive_MeleeDodgeChance, "aggressive_MeleeDodgeChance", 0.85f);
            Scribe_Values.Look(ref aggressive_MeleeDamageFactor, "aggressive_MeleeDamageFactor", 1.25f);
            Scribe_Values.Look(ref aggressive_MeleeCooldownFactor, "aggressive_MeleeCooldownFactor", 1.35f);
            Scribe_Values.Look(ref aggressive_ArmorPenetration, "aggressive_ArmorPenetration", 1.10f);

            Scribe_Values.Look(ref aggressive_MeleeParryChance, "aggressive_MeleeParryChance", 0.6f);
            Scribe_Values.Look(ref aggressive_MeleeParryDamageReduction, "aggressive_MeleeParryDamageReduction", 0.4f);
            Scribe_Values.Look(ref aggressive_MeleeCounterChance, "aggressive_MeleeCounterChance", 0.75f);


            // ------ 迅捷 (Flurry) ------
            Scribe_Values.Look(ref flurry_MeleeHitChance, "flurry_MeleeHitChance", 0.95f);
            Scribe_Values.Look(ref flurry_MeleeDodgeChance, "flurry_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref flurry_MeleeDamageFactor, "flurry_MeleeDamageFactor", 0.8f);
            Scribe_Values.Look(ref flurry_MeleeCooldownFactor, "flurry_MeleeCooldownFactor", 0.75f);
            Scribe_Values.Look(ref flurry_ArmorPenetration, "flurry_ArmorPenetration", 0.9f);

            Scribe_Values.Look(ref flurry_MeleeParryChance, "flurry_MeleeParryChance", 1.0f);
            Scribe_Values.Look(ref flurry_MeleeParryDamageReduction, "flurry_MeleeParryDamageReduction", 0.5f);
            Scribe_Values.Look(ref flurry_MeleeCounterChance, "flurry_MeleeCounterChance", 0.5f);


            // ------ 防御 (Guard) ------
            Scribe_Values.Look(ref guard_MeleeHitChance, "guard_MeleeHitChance", 0.9f);
            Scribe_Values.Look(ref guard_MeleeDodgeChance, "guard_MeleeDodgeChance", 1.5f);
            Scribe_Values.Look(ref guard_MeleeDamageFactor, "guard_MeleeDamageFactor", 0.75f);
            Scribe_Values.Look(ref guard_MeleeCooldownFactor, "guard_MeleeCooldownFactor", 1.05f);
            Scribe_Values.Look(ref guard_ArmorPenetration, "guard_ArmorPenetration", 0.85f);

            Scribe_Values.Look(ref guard_MeleeParryChance, "guard_MeleeParryChance", 1.6f);
            Scribe_Values.Look(ref guard_MeleeParryDamageReduction, "guard_MeleeParryDamageReduction", 0.75f);
            Scribe_Values.Look(ref guard_MeleeCounterChance, "guard_MeleeCounterChance", 0.3f);


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

            aggressive_MeleeParryChance = 0.6f;
            aggressive_MeleeParryDamageReduction = 0.4f;
            aggressive_MeleeCounterChance = 0.75f;



            // ------ 迅捷 (Flurry) ------
            flurry_MeleeHitChance = 0.95f;
            flurry_MeleeDodgeChance = 1f;
            flurry_MeleeDamageFactor = 0.8f;
            flurry_MeleeCooldownFactor = 0.75f;
            flurry_ArmorPenetration = 0.9f;

            flurry_MeleeParryChance = 1.0f;
            flurry_MeleeParryDamageReduction = 0.5f;
            flurry_MeleeCounterChance = 0.5f;

            // ------ 防御 (Guard) ------
            guard_MeleeHitChance = 0.9f;
            guard_MeleeDodgeChance = 1.5f;
            guard_MeleeDamageFactor = 0.75f;
            guard_MeleeCooldownFactor = 1.05f;
            guard_ArmorPenetration = 0.85f;

            guard_MeleeParryChance = 1.6f;
            guard_MeleeParryDamageReduction = 0.75f;
            guard_MeleeCounterChance = 0.3f;
        }
    }
}
