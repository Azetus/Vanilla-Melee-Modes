using Verse;

namespace VMM_VanillaMeleeModes.Settings
{
    public class VanillaMeleeModesModSetting : ModSettings
    {
        #region Vanilla

        // ------------ Vanilla ------------
        // ------ 强攻 (Aggressive) ------
        public float aggressive_MeleeHitChance = 1.15f;
        public float aggressive_MeleeDodgeChance = 0.85f;
        public float aggressive_MeleeDamageFactor = 1.25f;
        public float aggressive_MeleeCooldownFactor = 1.35f;
        public float aggressive_ArmorPenetration = 1.10f;

        public float aggressive_MeleeParryAngle = 80f;
        public float aggressive_MeleeParryChance = 0.9f;
        public float aggressive_MeleeParryDamageReduction = 0.8f;
        public float aggressive_MeleeCounterChance = 1.5f;


        // ------ 迅捷 (Flurry) ------
        public float flurry_MeleeHitChance = 0.95f;
        public float flurry_MeleeDodgeChance = 1f;
        public float flurry_MeleeDamageFactor = 0.8f;
        public float flurry_MeleeCooldownFactor = 0.75f;
        public float flurry_ArmorPenetration = 0.9f;

        public float flurry_MeleeParryAngle = 100f;
        public float flurry_MeleeParryChance = 0.8f;
        public float flurry_MeleeParryDamageReduction = 0.7f;
        public float flurry_MeleeCounterChance = 0.6f;

        // ------ 防御 (Guard) ------
        public float guard_MeleeHitChance = 0.9f;
        public float guard_MeleeDodgeChance = 1.5f;
        public float guard_MeleeDamageFactor = 0.75f;
        public float guard_MeleeCooldownFactor = 1.05f;
        public float guard_ArmorPenetration = 0.85f;

        public float guard_MeleeParryAngle = 180f;
        public float guard_MeleeParryChance = 1.6f;
        public float guard_MeleeParryDamageReduction = 1.5f;
        public float guard_MeleeCounterChance = 0.7f;

        // ------ 基础设置 ------
        public bool enable_VMM_parry = true;
        public bool enable_VMM_counterattack = true;
        public bool enable_VMM_parryAndCounterattackForNpc = true;
        
        #endregion

        #region CE

        // ------------ Combat Extended ------------
        // ------ 强攻 (Aggressive) ------
        public float CE_aggressive_MeleeHitChance = 1.15f;
        public float CE_aggressive_MeleeDodgeChance = 0.85f;
        public float CE_aggressive_MeleeDamageFactor = 1.25f;
        public float CE_aggressive_MeleeCooldownFactor = 1.35f;
        public float CE_aggressive_ArmorPenetration = 1.10f;

        public float CE_aggressive_MeleeParryChance = 0.9f;
        public float CE_aggressive_MeleeCritChance = 1.5f;

        // ------ 迅捷 (Flurry) ------
        public float CE_flurry_MeleeHitChance = 0.95f;
        public float CE_flurry_MeleeDodgeChance = 1f;
        public float CE_flurry_MeleeDamageFactor = 0.8f;
        public float CE_flurry_MeleeCooldownFactor = 0.75f;
        public float CE_flurry_ArmorPenetration = 0.9f;

        public float CE_flurry_MeleeParryChance = 0.8f;
        public float CE_flurry_MeleeCritChance = 0.8f;

        // ------ 防御 (Guard) ------
        public float CE_guard_MeleeHitChance = 0.9f;
        public float CE_guard_MeleeDodgeChance = 1.5f;
        public float CE_guard_MeleeDamageFactor = 0.75f;
        public float CE_guard_MeleeCooldownFactor = 1.05f;
        public float CE_guard_ArmorPenetration = 0.85f;

        public float CE_guard_MeleeParryChance = 1.6f;
        public float CE_guard_MeleeCritChance = 1f;

        #endregion

        public override void ExposeData()
        {
            #region Vanilla ExposeData

            // ------------ Vanilla ------------
            // ------ 强攻 (Aggressive) ------
            Scribe_Values.Look(ref aggressive_MeleeHitChance, "aggressive_MeleeHitChance", 1.15f);
            Scribe_Values.Look(ref aggressive_MeleeDodgeChance, "aggressive_MeleeDodgeChance", 0.85f);
            Scribe_Values.Look(ref aggressive_MeleeDamageFactor, "aggressive_MeleeDamageFactor", 1.25f);
            Scribe_Values.Look(ref aggressive_MeleeCooldownFactor, "aggressive_MeleeCooldownFactor", 1.35f);
            Scribe_Values.Look(ref aggressive_ArmorPenetration, "aggressive_ArmorPenetration", 1.10f);

            Scribe_Values.Look(ref aggressive_MeleeParryAngle, "aggressive_MeleeParryAngle", 80f);
            Scribe_Values.Look(ref aggressive_MeleeParryChance, "aggressive_MeleeParryChance", 0.9f);
            Scribe_Values.Look(ref aggressive_MeleeParryDamageReduction, "aggressive_MeleeParryDamageReduction", 0.8f);
            Scribe_Values.Look(ref aggressive_MeleeCounterChance, "aggressive_MeleeCounterChance", 1.5f);


            // ------ 迅捷 (Flurry) ------
            Scribe_Values.Look(ref flurry_MeleeHitChance, "flurry_MeleeHitChance", 0.95f);
            Scribe_Values.Look(ref flurry_MeleeDodgeChance, "flurry_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref flurry_MeleeDamageFactor, "flurry_MeleeDamageFactor", 0.8f);
            Scribe_Values.Look(ref flurry_MeleeCooldownFactor, "flurry_MeleeCooldownFactor", 0.75f);
            Scribe_Values.Look(ref flurry_ArmorPenetration, "flurry_ArmorPenetration", 0.9f);

            Scribe_Values.Look(ref flurry_MeleeParryAngle, "flurry_MeleeParryAngle", 100f);
            Scribe_Values.Look(ref flurry_MeleeParryChance, "flurry_MeleeParryChance", 0.8f);
            Scribe_Values.Look(ref flurry_MeleeParryDamageReduction, "flurry_MeleeParryDamageReduction", 0.7f);
            Scribe_Values.Look(ref flurry_MeleeCounterChance, "flurry_MeleeCounterChance", 0.6f);


            // ------ 防御 (Guard) ------
            Scribe_Values.Look(ref guard_MeleeHitChance, "guard_MeleeHitChance", 0.9f);
            Scribe_Values.Look(ref guard_MeleeDodgeChance, "guard_MeleeDodgeChance", 1.5f);
            Scribe_Values.Look(ref guard_MeleeDamageFactor, "guard_MeleeDamageFactor", 0.75f);
            Scribe_Values.Look(ref guard_MeleeCooldownFactor, "guard_MeleeCooldownFactor", 1.05f);
            Scribe_Values.Look(ref guard_ArmorPenetration, "guard_ArmorPenetration", 0.85f);

            Scribe_Values.Look(ref guard_MeleeParryAngle, "guard_MeleeParryAngle", 180f);
            Scribe_Values.Look(ref guard_MeleeParryChance, "guard_MeleeParryChance", 1.6f);
            Scribe_Values.Look(ref guard_MeleeParryDamageReduction, "guard_MeleeParryDamageReduction", 1.5f);
            Scribe_Values.Look(ref guard_MeleeCounterChance, "guard_MeleeCounterChance", 0.7f);

            // ------ 基础设置 ------
            Scribe_Values.Look(ref enable_VMM_parry, "enable_VMM_parry", true);
            Scribe_Values.Look(ref enable_VMM_counterattack, "enable_VMM_counterattack", true);
            Scribe_Values.Look(ref enable_VMM_parryAndCounterattackForNpc, "enable_VMM_parryAndCounterattackForNpc", true);
            
            #endregion

            #region CE ExposeData

            // ------------ Combat Extended ------------
            // ------ 强攻 (Aggressive) ------
            Scribe_Values.Look(ref CE_aggressive_MeleeHitChance, "CE_aggressive_MeleeHitChance", 1.15f);
            Scribe_Values.Look(ref CE_aggressive_MeleeDodgeChance, "CE_aggressive_MeleeDodgeChance", 0.85f);
            Scribe_Values.Look(ref CE_aggressive_MeleeDamageFactor, "CE_aggressive_MeleeDamageFactor", 1.25f);
            Scribe_Values.Look(ref CE_aggressive_MeleeCooldownFactor, "CE_aggressive_MeleeCooldownFactor", 1.35f);
            Scribe_Values.Look(ref CE_aggressive_ArmorPenetration, "CE_aggressive_ArmorPenetration", 1.10f);
            Scribe_Values.Look(ref CE_aggressive_MeleeParryChance, "CE_aggressive_MeleeParryChance", 0.9f);
            Scribe_Values.Look(ref CE_aggressive_MeleeCritChance, "CE_aggressive_MeleeCritChance", 1.5f);

            // ------ 迅捷 (Flurry) ------
            Scribe_Values.Look(ref CE_flurry_MeleeHitChance, "CE_flurry_MeleeHitChance", 0.95f);
            Scribe_Values.Look(ref CE_flurry_MeleeDodgeChance, "CE_flurry_MeleeDodgeChance", 1f);
            Scribe_Values.Look(ref CE_flurry_MeleeDamageFactor, "CE_flurry_MeleeDamageFactor", 0.8f);
            Scribe_Values.Look(ref CE_flurry_MeleeCooldownFactor, "CE_flurry_MeleeCooldownFactor", 0.75f);
            Scribe_Values.Look(ref CE_flurry_ArmorPenetration, "CE_flurry_ArmorPenetration", 0.9f);
            Scribe_Values.Look(ref CE_flurry_MeleeParryChance, "CE_flurry_MeleeParryChance", 0.8f);
            Scribe_Values.Look(ref CE_flurry_MeleeCritChance, "CE_flurry_MeleeCritChance", 0.8f);

            // ------ 防御 (Guard) ------
            Scribe_Values.Look(ref CE_guard_MeleeHitChance, "CE_guard_MeleeHitChance", 0.9f);
            Scribe_Values.Look(ref CE_guard_MeleeDodgeChance, "CE_guard_MeleeDodgeChance", 1.5f);
            Scribe_Values.Look(ref CE_guard_MeleeDamageFactor, "CE_guard_MeleeDamageFactor", 0.75f);
            Scribe_Values.Look(ref CE_guard_MeleeCooldownFactor, "CE_guard_MeleeCooldownFactor", 1.05f);
            Scribe_Values.Look(ref CE_guard_ArmorPenetration, "CE_guard_ArmorPenetration", 0.85f);
            Scribe_Values.Look(ref CE_guard_MeleeParryChance, "CE_guard_MeleeParryChance", 1.6f);
            Scribe_Values.Look(ref CE_guard_MeleeCritChance, "CE_guard_MeleeCritChance", 1f);

            #endregion

            base.ExposeData();
        }

        public void ResetSetting()
        {
            // ------------ Vanilla ------------
            // ------ 强攻 (Aggressive) ------
            aggressive_MeleeHitChance = 1.15f;
            aggressive_MeleeDodgeChance = 0.85f;
            aggressive_MeleeDamageFactor = 1.25f;
            aggressive_MeleeCooldownFactor = 1.35f;
            aggressive_ArmorPenetration = 1.10f;

            aggressive_MeleeParryAngle = 80f;
            aggressive_MeleeParryChance = 0.9f;
            aggressive_MeleeParryDamageReduction = 0.8f;
            aggressive_MeleeCounterChance = 1.5f;


            // ------ 迅捷 (Flurry) ------
            flurry_MeleeHitChance = 0.95f;
            flurry_MeleeDodgeChance = 1f;
            flurry_MeleeDamageFactor = 0.8f;
            flurry_MeleeCooldownFactor = 0.75f;
            flurry_ArmorPenetration = 0.9f;

            flurry_MeleeParryAngle = 100f;
            flurry_MeleeParryChance = 0.8f;
            flurry_MeleeParryDamageReduction = 0.7f;
            flurry_MeleeCounterChance = 0.6f;

            // ------ 防御 (Guard) ------
            guard_MeleeHitChance = 0.9f;
            guard_MeleeDodgeChance = 1.5f;
            guard_MeleeDamageFactor = 0.75f;
            guard_MeleeCooldownFactor = 1.05f;
            guard_ArmorPenetration = 0.85f;

            guard_MeleeParryAngle = 180f;
            guard_MeleeParryChance = 1.6f;
            guard_MeleeParryDamageReduction = 1.5f;
            guard_MeleeCounterChance = 0.7f;
            
        // ------ 基础设置 ------
            enable_VMM_parry = true;
            enable_VMM_counterattack = true;
            enable_VMM_parryAndCounterattackForNpc = true;
        }

        public void ResetSetting_CE()
        {
            // ------------ Combat Extended ------------
            // ------ 强攻 (Aggressive) ------
            CE_aggressive_MeleeHitChance = 1.15f;
            CE_aggressive_MeleeDodgeChance = 0.85f;
            CE_aggressive_MeleeDamageFactor = 1.25f;
            CE_aggressive_MeleeCooldownFactor = 1.35f;
            CE_aggressive_ArmorPenetration = 1.10f;
            CE_aggressive_MeleeParryChance = 0.9f;
            CE_aggressive_MeleeCritChance = 1.5f;

            // ------ 迅捷 (Flurry) ------
            CE_flurry_MeleeHitChance = 0.95f;
            CE_flurry_MeleeDodgeChance = 1f;
            CE_flurry_MeleeDamageFactor = 0.8f;
            CE_flurry_MeleeCooldownFactor = 0.75f;
            CE_flurry_ArmorPenetration = 0.9f;
            CE_flurry_MeleeParryChance = 0.8f;
            CE_flurry_MeleeCritChance = 0.8f;

            // ------ 防御 (Guard) ------
            CE_guard_MeleeHitChance = 0.9f;
            CE_guard_MeleeDodgeChance = 1.5f;
            CE_guard_MeleeDamageFactor = 0.75f;
            CE_guard_MeleeCooldownFactor = 1.05f;
            CE_guard_ArmorPenetration = 0.85f;
            CE_guard_MeleeParryChance = 1.6f;
            CE_guard_MeleeCritChance = 1f;
        }
    }
}