using RimWorld;
using Verse;

namespace VMM_VanillaMeleeModes.Stat
{
    public class VMM_StatWorker:StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (!req.HasThing || req.Thing is not Pawn pawn)
                return false;

            return pawn.RaceProps.Humanlike; // 只显示人类
        }

        public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
        {
            if (!req.HasThing || req.Thing is not Pawn pawn)
                return 0f;

            if (!pawn.RaceProps.Humanlike)
                return 0f;

            return base.GetValueUnfinalized(req, applyPostProcess);
        }
    }
}
