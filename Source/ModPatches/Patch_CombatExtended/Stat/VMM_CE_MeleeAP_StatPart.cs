using RimWorld;
using Verse;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended.Stat
{
    public class VMM_CE_MeleeAP_StatPart : VMM_CE_MeleeMode_StatPart
    {
        protected override float? GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB_CE.GetMeleeArmorPenetration_CE(mode);
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if (comp == null) return;
            float? factor = GetFactor(comp.curMode);
            if (factor.HasValue)
            {
                ApplyValue(ref val, factor.Value);
            }
        }

        public override string? ExplanationPart(StatRequest req)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if (comp == null) return null;
            var factor = GetFactor(comp.curMode);

            if (factor.HasValue){
                return "VMM_StatPart_Label".Translate(
                    Utils.GetMeleeModeLabelFor(comp.curMode),
                    Utils.ToPercentString(factor.Value)
                );
            }
            return null;
        }

        protected override VMM_PawnCompMeleeMode? TryGetComp(StatRequest req)
        {
            if (!req.HasThing)
                return null;

            var thing = req.Thing;
            if (thing is not ThingWithComps thingWithComps)
                return null;

            if (thingWithComps.ParentHolder is Pawn_EquipmentTracker tracker)
            {
                Pawn pawn = tracker.pawn;
                return pawn.TryGetComp<VMM_PawnCompMeleeMode>();
            }

            return null;
        }
    }
}