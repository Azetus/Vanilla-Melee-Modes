using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.DefOfs
{
    [DefOf]
    public static class VMM_HediffDefOf
    {
        public static HediffDef VMM_CounterAttackCooldown;

        static VMM_HediffDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(VMM_HediffDefOf));
        }
    }
}
