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

        public static bool isCEActive = false;

        public VanillaMeleeModes(ModContentPack contentPack) : base(contentPack)
        {
            settings = GetSettings<VanillaMeleeModesModSetting>();
            isCEActive = ModLister.GetActiveModWithIdentifier("CETeam.CombatExtended") != null;
            
            // Mod针对原版环境添加了格挡反击并修改了护甲穿透，检测到CE加载时不要Patch这部分内容
            if (!isCEActive)
            {
                new Harmony("Aliza.VanillaMeleeModes").PatchAll();
                Log.Message("<color=cyan>[VanillaMeleeModes]</color> applying vanilla patches.");
            }
            
            Log.Message("<color=cyan>[VanillaMeleeModes]</color> is loaded!");
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