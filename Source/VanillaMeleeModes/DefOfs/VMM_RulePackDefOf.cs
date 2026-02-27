using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.DefOfs
{
    [DefOf]

    public static class VMM_RulePackDefOf
    {

        public static RulePackDef VMM_Melee_Parried;
        public static RulePackDef VMM_Melee_CounterAttack;

        static VMM_RulePackDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(VMM_RulePackDefOf));
        }
    }
}
