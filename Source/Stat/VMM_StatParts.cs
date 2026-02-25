using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Stat
{
    // --- Vanilla ---
    // 原版近战命中率
    public class VMM_MeleeMode_MeleeHitChance_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeHitChance(mode);
        }
    }

    // 原版近战闪避率
    public class VMM_MeleeMode_MeleeDodgeChance_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeDodgeChance(mode);
        }
    }

    // 原版近战伤害
    public class VMM_MeleeMode_MeleeDamageFactor_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeDamageFactor(mode);
        }
    }

    // 原版近战冷却
    public class VMM_MeleeMode_MeleeCooldownFactor_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeCooldownFactor(mode);
        }
    }

    // --- Vanilla Parry ---
    // 格挡率
    public class VMM_MeleeMode_MeleeParryChance_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeParryChanceFactor(mode);
        }
    }

    // 格挡减伤
    public class VMM_MeleeMode_MeleeParryDamageReduction_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeParryDamageReductionFactor(mode);
        }
    }

    // 格挡反击率
    public class VMM_MeleeMode_MeleeCounterChance_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeCounterChanceFactor(mode);
        }
    }
}
