using HarmonyLib;
using Verse;
using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes
{
    public class VanillaMeleeModes : Mod
    {
        public static VanillaMeleeModesModSetting settings;
        public VanillaMeleeModes(ModContentPack contentPack) : base(contentPack)
        {
            settings = GetSettings<VanillaMeleeModesModSetting>();
            Log.Message("[VanillaMeleeModes] is loaded!");
            new Harmony("Aliza.VanillaMeleeModes").PatchAll();
        }
    }
}
