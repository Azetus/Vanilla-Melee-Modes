using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.Stat
{
    [StaticConstructorOnStartup]
    public static class VMM_StatPatcher
    {
        static VMM_StatPatcher()
        {
            // CE也使用了这些原版的StatDef，为了避免重复叠加，CE有一份单独StatPart
            if (!VanillaMeleeModes.isCEActive)
            {
                InjectPart(StatDefOf.MeleeHitChance, new VMM_MeleeMode_MeleeHitChance_FactorPart());
                InjectPart(StatDefOf.MeleeDodgeChance, new VMM_MeleeMode_MeleeDodgeChance_FactorPart());
                InjectPart(StatDefOf.MeleeDamageFactor, new VMM_MeleeMode_MeleeDamageFactor_FactorPart());
                InjectPart(StatDefOf.MeleeCooldownFactor, new VMM_MeleeMode_MeleeCooldownFactor_FactorPart());
            }
        }

        private static void InjectPart<T>(StatDef stat, T part) where T : VMM_MeleeMode_StatPart, new()
        {
            if (stat.parts == null)
                stat.parts = new List<StatPart>();
            if (!stat.parts.Any(p => p is T))
                stat.parts.Add(part);
        }
    }
}