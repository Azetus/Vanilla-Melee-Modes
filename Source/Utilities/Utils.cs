using Verse;
using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Utilities
{
    internal class Utils
    {
        public static string GetMeleeModeLabelFor(VMM_MeleeMode mode)
        {
            switch (mode)
            {
                case VMM_MeleeMode.Aggressive: return "VMM_AggressiveMode".Translate();
                case VMM_MeleeMode.Flurry: return "VMM_FlurryMode".Translate();
                case VMM_MeleeMode.Guard: return "VMM_GuardMode".Translate();
                default: return "VMM_DefaultMode".Translate();
            }
        }

        public static string ToPercentString(float value)
        {
            return $"{value * 100f}%";
        }
    }
}
