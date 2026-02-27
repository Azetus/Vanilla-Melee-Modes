using HarmonyLib;
using Verse;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended
{
    [StaticConstructorOnStartup]
    public static class VMM_PatchCore_CombatExtended
    {
        static VMM_PatchCore_CombatExtended()
        {
            if (VanillaMeleeModes.isCEActive)
            {
                ApplyPatches();
            }
            else
            {
                Log.Message("<color=cyan>[VanillaMeleeModes-CE-patch]</color> CE not detected as active, skipping patches.");
            }
        }

        private static void ApplyPatches()
        {
            try
            {
                var harmony = new Harmony("Aliza.VanillaMeleeModes.CombatExtended.Compatibility");
                harmony.PatchAll();
                Log.Message("<color=cyan>[VanillaMeleeModes-CE-patch]</color> Successfully applied Combat Extended compatibility patches.");
            }
            catch (Exception ex)
            {
                Log.Error($"<color=cyan>[VanillaMeleeModes-CE-patch]</color> Failed to apply compatibility patches: {ex.Message}");
            }
        }
    }
}