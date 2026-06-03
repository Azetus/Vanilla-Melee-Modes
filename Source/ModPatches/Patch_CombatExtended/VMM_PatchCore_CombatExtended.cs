using Verse;
using VMM_VanillaMeleeModes.Patch_CombatExtended.Utilities;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended
{
    [StaticConstructorOnStartup]
    public static class VMM_PatchCore_CombatExtended
    {
        static VMM_PatchCore_CombatExtended()
        {
            AutoModeEvaluator.RegisterCEEvaluator(CEEvaluator.Evaluate);
            Log.Message("<color=cyan>[VanillaMeleeModes-CE]</color> CE auto-evaluation registered.");
        }
    }
}