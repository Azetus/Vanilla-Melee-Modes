using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended.Stat
{
    // --- CE ---
    // --- CE近战命中率 ---
    public class VMM_CE_MeleeMode_MeleeHitChance_FactorPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeHitChance_CE(mode);
        }
    }

    // --- CE近战闪避率 ---
    public class VMM_CE_MeleeMode_MeleeDodgeChance_FactorPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeDodgeChance_CE(mode);
        }
    }

    // --- CE近战伤害倍率 ---
    public class VMM_CE_MeleeMode_MeleeDamageFactor_FactorPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeDamageFactor_CE(mode);
        }
    }

    // --- CE近战冷却倍率 ---
    public class VMM_CE_MeleeMode_MeleeCooldownFactor_FactorPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeCooldownFactor_CE(mode);
        }
    }


    // ----- CE的StatDefOf
    // --- CE近战暴击率 --- CE_StatDefOf.MeleeCritChance
    public class VMM_CE_MeleeMode_MeleeCritChance_FactorPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeCritChanceFactor_CE(mode);
        }
    }

    // --- CE的近战格挡率 --- CE_StatDefOf.MeleeParryChance
    public class VMM_CE_MeleeMode_MeleeParryChance_FactorPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeParryChanceFactor_CE(mode);
        }
    }
}