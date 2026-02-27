using CombatExtended;
using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended.Stat
{
    [StaticConstructorOnStartup]
    public static class VMM_CE_StatPatcher
    {
        static VMM_CE_StatPatcher()
        {
            // 检测到CE加载后再注入
            if (VanillaMeleeModes.isCEActive)
            {
                // CE的近战命中率、闪避率、伤害、冷却时间同样使用了原版的StatDef
                InjectPart(StatDefOf.MeleeHitChance, new VMM_CE_MeleeMode_MeleeHitChance_FactorPart());
                InjectPart(StatDefOf.MeleeDodgeChance, new VMM_CE_MeleeMode_MeleeDodgeChance_FactorPart());
                InjectPart(StatDefOf.MeleeDamageFactor, new VMM_CE_MeleeMode_MeleeDamageFactor_FactorPart());
                InjectPart(StatDefOf.MeleeCooldownFactor, new VMM_CE_MeleeMode_MeleeCooldownFactor_FactorPart());
                
                // CE的StatDef
                InjectPart(CE_StatDefOf.MeleeCritChance, new VMM_CE_MeleeMode_MeleeCritChance_FactorPart());
                InjectPart(CE_StatDefOf.MeleeParryChance, new VMM_CE_MeleeMode_MeleeParryChance_FactorPart());
                InjectPart(CE_StatDefOf.MeleePenetrationFactor, new VMM_CE_MeleeAP_StatPart());
            }
        }

        private static void InjectPart<T>(StatDef stat, T part) where T : VMM_CE_MeleeMode_StatPart, new()
        {
            if (stat.parts == null)
                stat.parts = new List<StatPart>();
            if (!stat.parts.Any(p => p is T))
                stat.parts.Add(part);
        }
    }
}