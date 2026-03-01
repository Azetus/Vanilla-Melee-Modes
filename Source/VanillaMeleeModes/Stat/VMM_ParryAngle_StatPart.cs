using RimWorld;
using Verse;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Stat
{
    public class VMM_ParryAngle_StatPart : VMM_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeParryAngleValue(mode);
        }
        
        public override string? ExplanationPart(StatRequest req)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if (comp == null) return null;
            var factor = GetFactor(comp.curMode);

            if (factor.HasValue)
            {
                return "VMM_StatPart_Label".Translate(
                    Utils.GetMeleeModeLabelFor(comp.curMode),
                    factor.Value
                );
            }

            return null;
        }

        protected override void ApplyValue(ref float val, float factor)
        {
            val = factor;
        }
    }
}