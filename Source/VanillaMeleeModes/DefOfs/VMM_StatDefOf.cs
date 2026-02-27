using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.DefOfs
{
    [StaticConstructorOnStartup]
    public static class VMM_StatDefOf
    {

        public static StatDef VMM_MeleeParryChance;
        public static StatDef VMM_MeleeParryDamageReduction;
        public static StatDef VMM_MeleeCounterChance;

        static VMM_StatDefOf()
        {
            if(VanillaMeleeModes.isCEActive)
                return;
            VMM_MeleeParryChance = DefDatabase<StatDef>.GetNamed("VMM_MeleeParryChance", true);
            VMM_MeleeParryDamageReduction = DefDatabase<StatDef>.GetNamed("VMM_MeleeParryDamageReduction", true);
            VMM_MeleeCounterChance = DefDatabase<StatDef>.GetNamed("VMM_MeleeCounterChance", true);
        }
    }
}
