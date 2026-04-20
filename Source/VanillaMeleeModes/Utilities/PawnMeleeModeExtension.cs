using Verse;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Utilities
{
    public static class PawnMeleeModeExtension
    {
        public static VMM_MeleeMode VMM_GetMeleeMode(this Pawn pawn)
        {
            if (pawn == null)
                return VMM_MeleeMode.Default;
            var comp = pawn.TryGetComp<VMM_PawnCompMeleeMode>();

            return comp?.curMode ?? VMM_MeleeMode.Default;
        }

        public static void VMM_SetMeleeMode(this Pawn pawn, VMM_MeleeMode mode)
        {
            if (pawn == null)
                return;
            var comp = pawn.TryGetComp<VMM_PawnCompMeleeMode>();

            if (comp is VMM_PawnCompMeleeMode VMM_Comp)
            {
                VMM_Comp.curMode = mode;
            }
        }
        
        public static bool VMM_enableAutoSelection(this Pawn pawn) {
            if(pawn == null)
                return false;

            var comp = pawn.TryGetComp<VMM_PawnCompMeleeMode>();

            return comp?.curEnableAutoSelection ?? false;
        }
    }
}