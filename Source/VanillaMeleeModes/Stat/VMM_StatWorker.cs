using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.Stat
{
    public class VMM_StatWorker : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (!req.HasThing || req.Thing is not Pawn pawn)
                return false;

            return pawn.RaceProps.Humanlike || pawn.RaceProps.IsMechanoid; // 只显示人类和机械族
        }

        public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
        {
            if (!req.HasThing || req.Thing is not Pawn pawn)
                return 0f;

            if (!(pawn.RaceProps.Humanlike || pawn.RaceProps.IsMechanoid))
                return 0f;

            return base.GetValueUnfinalized(req, applyPostProcess);
        }
    }
}