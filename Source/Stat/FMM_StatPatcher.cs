using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.Stat
{
    [StaticConstructorOnStartup]
    public static class FMM_StatPatcher
    {
        static FMM_StatPatcher()
        {
            InjectPart(StatDefOf.MeleeHitChance, new VMM_MeleeMode_MeleeHitChance_FactorPart());
            InjectPart(StatDefOf.MeleeDodgeChance, new VMM_MeleeMode_MeleeDodgeChance_FactorPart());
            InjectPart(StatDefOf.MeleeDamageFactor, new VMM_MeleeMode_MeleeDamageFactor_FactorPart());
            InjectPart(StatDefOf.MeleeCooldownFactor, new VMM_MeleeMode_MeleeCooldownFactor_FactorPart());

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
