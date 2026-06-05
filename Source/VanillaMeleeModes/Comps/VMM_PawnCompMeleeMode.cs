using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Comps
{
    public class VMM_PawnCompMeleeMode : ThingComp
    {
        private VMM_MeleeMode mode = VMM_MeleeMode.Default;

        // 自动模式状态
        private bool isAutoMode = false;

        // 上一次玩家手动触发自动切换评估的时间戳（用于冷却计算）
        private int lastPlayerOverrideTick = -9999;

        // 自动切换常量
        private const int AUTO_POLL_INTERVAL = 30;
        private const int AUTO_COOLDOWN_TICKS = 90;

        public VMM_MeleeMode curMode
        {
            get => mode;
            set => mode = value;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref mode, "VMM_meleeMode", VMM_MeleeMode.Default);
            Scribe_Values.Look(ref isAutoMode, "VMM_isAutoMode", false);
        }

        // 统一轮询轨：每30tick触发，战斗中评估（迟滞抑制振荡）
        public override void CompTick()
        {
            base.CompTick();

            if (!(parent is Pawn pawn))
                return;
            // 第一层：仅自动模式下评估
            if (!IsAutoModeEffective(pawn))
                return;
            // 第二层：节流错峰
            if (!pawn.IsHashIntervalTick(AUTO_POLL_INTERVAL))
                return;
            // 第三层：仅战斗中评估
            if (!AutoModeEvaluator.IsInMeleeCombat(pawn))
                return;
            // 第四层：冷却期内仅允许紧急Guard模式越级
            if (GenTicks.TicksGame - lastPlayerOverrideTick < AUTO_COOLDOWN_TICKS)
            {
                if (AutoModeEvaluator.ShouldTriggerEmergencyGuard(pawn)
                    && curMode != VMM_MeleeMode.Guard)
                {
                    curMode = VMM_MeleeMode.Guard;
                }

                return;
            }

            EvaluateAndSwitch(pawn);
        }

        // 玩家响应轨入口（由Patch_AutoMode_OnPlayerMeleeJob调用）
        public void OnPlayerStartedMeleeJob()
        {
            if (!(parent is Pawn pawn))
                return;
            if (!isAutoMode)
                return;
            if (!MeleeModeDB.Settings.enableAutoSelectionForPlayer)
                return;

            EvaluateAndSwitch(pawn);
            lastPlayerOverrideTick = GenTicks.TicksGame;
        }

        // 执行评估并切换模式
        private void EvaluateAndSwitch(Pawn pawn)
        {
            Thing? target = AutoModeEvaluator.GetCombatTarget(pawn);
            VMM_MeleeMode result = AutoModeEvaluator.Evaluate(pawn, target, curMode);
            if (result != curMode)
                curMode = result;
        }

        // 判断当前Pawn是否应启用自动评估
        private bool IsAutoModeEffective(Pawn pawn)
        {
            if (pawn.Faction == Faction.OfPlayer)
                return isAutoMode && MeleeModeDB.Settings.enableAutoSelectionForPlayer;

            return MeleeModeDB.Settings.enableAutoSelectionForPlayer
                   && MeleeModeDB.Settings.enableAutoSelectionForNPC;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent is not Pawn pawn)
                yield break;

            bool show = (pawn.IsColonistPlayerControlled
                         || pawn.IsColonyMechPlayerControlled
                         || pawn.IsColonySubhumanPlayerControlled)
                        && (pawn.Drafted || MeleeModeDB.Settings.alwaysDisplayGizmo);

            if (!show)
                yield break;

            // Auto切换按钮
            if (MeleeModeDB.Settings.enableAutoSelectionForPlayer)
            {
                yield return new Command_Toggle
                {
                    icon = VMM_IconTexture.VMM_Auto_Icon,
                    defaultLabel = "VMM_AutoMode".Translate(),
                    defaultDesc = "VMM_AutoMode_Desc".Translate(),
                    isActive = () => isAutoMode,
                    toggleAction = () =>
                    {
                        isAutoMode = !isAutoMode;
                        curMode = VMM_MeleeMode.Default;
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                };
            }

            // 手动循环按钮（Auto关闭时显示）
            if (!isAutoMode || !MeleeModeDB.Settings.enableAutoSelectionForPlayer)
            {
                yield return new Command_Action
                {
                    icon = GetIconFor(curMode),
                    defaultLabel = Utils.GetMeleeModeLabelFor(curMode),
                    defaultDesc = "VMM_SwitchGizmoDesc".Translate(),
                    action = () =>
                    {
                        curMode = (VMM_MeleeMode)(((int)curMode + 1) % 4);
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                };
            }
        }

        private Texture2D GetIconFor(VMM_MeleeMode m)
        {
            switch (m)
            {
                case VMM_MeleeMode.Default: return VMM_IconTexture.VMM_Default_Icon;
                case VMM_MeleeMode.Aggressive: return VMM_IconTexture.VMM_Aggressive_Icon;
                case VMM_MeleeMode.Flurry: return VMM_IconTexture.VMM_Flurry_Icon;
                case VMM_MeleeMode.Guard: return VMM_IconTexture.VMM_Guard_Icon;
                default: return VMM_IconTexture.VMM_Default_Icon;
            }
        }
    }
}