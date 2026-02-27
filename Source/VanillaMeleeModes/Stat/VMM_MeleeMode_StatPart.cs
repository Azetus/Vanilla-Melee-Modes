using RimWorld;
using Verse;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Stat
{
    public abstract class VMM_MeleeMode_StatPart : StatPart
    {
        protected abstract float GetFactor(VMM_MeleeMode mode);

        public override void TransformValue(StatRequest req, ref float val)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if(comp == null) return;
            val *= GetFactor(comp.curMode);
        }

        public override string? ExplanationPart(StatRequest req)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if (comp == null) return null;
            var factor = GetFactor(comp.curMode);

            return "VMM_StatPart_Label".Translate(
                    Utils.GetMeleeModeLabelFor(comp.curMode),
                    Utils.ToPercentString(factor)
                );
        }

        private static VMM_PawnCompMeleeMode? TryGetComp(StatRequest req) {
            if(!req.HasThing) return null;
            if(req.Thing is not Pawn pawn) return null;

            return pawn.TryGetComp<VMM_PawnCompMeleeMode>();
        }


    }

}
