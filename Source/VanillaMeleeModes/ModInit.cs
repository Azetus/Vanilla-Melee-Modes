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
            
            var harmony = new Harmony("Aliza.VanillaMeleeModes");

            if (!isCEActive)
            {
                // 原版模式：加载全部补丁
                harmony.PatchAll();
                Log.Message("<color=cyan>[VanillaMeleeModes]</color> applying vanilla patches.");
            }
            else
            {
                // CE 模式：仅加载自动切换功能的 StartJob 入口
                harmony.CreateClassProcessor(typeof(Patches.Patch_AutoMode_OnPlayerMeleeJob)).Patch();
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