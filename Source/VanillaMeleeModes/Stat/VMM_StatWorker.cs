using RimWorld;
using Verse;
using VMM_VanillaMeleeModes.DefOfs;

namespace VMM_VanillaMeleeModes.Stat
{
    // VMM添加的StatDef所用的StatWorker
    public class VMM_StatWorker : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            if (!base.ShouldShowFor(req))
            {
                return false;
            }
            
            if (!req.HasThing || req.Thing is not Pawn pawn)
                return false;

            // 有CE时，用到这几个Stat的Patch不会生效，虽然StatDef会被加载但不会被使用
            // 检测到CE加载的话就关闭显示，避免产生误解
            if (VanillaMeleeModes.isCEActive)
                return false;

            //检测下Mod Setting
            //关闭格挡机制就隐藏
            if(!VanillaMeleeModes.settings.enable_VMM_parry)
                return false;
            //关闭反击机制就隐藏
            if(!VanillaMeleeModes.settings.enable_VMM_counterattack && this.stat == VMM_StatDefOf.VMM_MeleeCounterChance)
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