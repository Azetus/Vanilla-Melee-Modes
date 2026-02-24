using HarmonyLib;
using UnityEngine;
using Verse;
using VMM_VanillaMeleeModes.ModSettingUI;
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

        public override string SettingsCategory()
        {
            return "VMM_ModTitle".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            VMM_SettingsWindowContents.SettingsWindowContents(inRect, ref settings);
        }
    }
}
