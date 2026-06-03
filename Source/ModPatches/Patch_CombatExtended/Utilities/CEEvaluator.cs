using Verse;
using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended.Utilities
{
    public static class CEEvaluator
    {
        public static VMM_MeleeMode Evaluate(Pawn pawn, Thing? target,
            VMM_MeleeMode currentMode)
        {
            // TODO: CE 值域评分实现
            // Tier 1: 紧急规则（与原版完全相同）
            // Tier 2: CE 加权评分（护甲 mm RHA / 暴击 / CE 闪避）
            return currentMode;
        }
    }
}