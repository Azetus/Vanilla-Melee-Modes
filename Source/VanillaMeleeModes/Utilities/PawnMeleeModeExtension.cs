using Verse;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Utilities
{
    public static class PawnMeleeModeExtension
    {
        public static VMM_MeleeMode VMM_GetMeleeMode(this Pawn pawn)
        {
            if(pawn == null)
                return VMM_MeleeMode.Default;
            var comp = pawn.TryGetComp<VMM_PawnCompMeleeMode>();

            return comp?.curMode ?? VMM_MeleeMode.Default;
        }
    }
}
