using RimWorld;

namespace VMM_VanillaMeleeModes.DefOfs
{
    [DefOf]
    public static class VMM_StatDefOf
    {

        public static StatDef VMM_MeleeParryChance;
        public static StatDef VMM_MeleeParryDamageReduction;
        public static StatDef VMM_MeleeCounterChance;

        static VMM_StatDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(VMM_StatDefOf));
        }
    }
}
