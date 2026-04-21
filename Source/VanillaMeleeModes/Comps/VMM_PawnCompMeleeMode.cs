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

        private bool enableAutoSelection = false;

        private int lastSwitchTick = 0;

        // CompTick 执行频率
        private static readonly int _compTickInterval = 30;

        // 模式切换冷却 Ticks, 防止切换触发入口之间冲突
        private static readonly int _switchModeCoolDown = 30;


        public VMM_MeleeMode curMode
        {
            get => mode;
            set => mode = value;
        }

        public bool curEnableAutoSelection
        {
            get => enableAutoSelection;
            set => enableAutoSelection = value;
        }

        public int LastSwitchTick
        {
            get => lastSwitchTick;
            set => lastSwitchTick = value;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref mode, "VMM_meleeMode", VMM_MeleeMode.Default);
            Scribe_Values.Look(ref enableAutoSelection, "VMM_autoSelection", true);
            Scribe_Values.Look(ref lastSwitchTick, "VMM_lastSwitchTick", 0);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent is Pawn pawn &&
                (pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled || pawn.IsColonySubhumanPlayerControlled) &&
                (pawn.Drafted || MeleeModeDB.Settings.alwaysDisplayGizmo)
               )
            {
                // TODO: 测试结束后，加上 !curEnableAutoSelection ，自动切换开启时不要显示手动切换按钮
                if (!MeleeModeDB.Settings.enableAutoSelectionForPlayer)
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

                if (MeleeModeDB.Settings.enableAutoSelectionForPlayer)
                {
                    yield return new Command_Toggle
                    {
                        icon = VMM_IconTexture.VMM_Auto_Icon,
                        defaultLabel = "VMM_AutoSelection".Translate(),
                        defaultDesc = "VMM_AutoSelection_Desc".Translate(),
                        isActive = () => curEnableAutoSelection,
                        toggleAction = () =>
                        {
                            curEnableAutoSelection = !curEnableAutoSelection;
                            // curMode = VMM_MeleeMode.Default; // TODO: 测试完之后解除注释
                            SoundDefOf.Tick_High.PlayOneShotOnCamera();
                        }
                    };
                }
            }
        }


        private Texture2D GetIconFor(VMM_MeleeMode mode)
        {
            switch (mode)
            {
                case VMM_MeleeMode.Default: return VMM_IconTexture.VMM_Default_Icon;
                case VMM_MeleeMode.Aggressive: return VMM_IconTexture.VMM_Aggressive_Icon;
                case VMM_MeleeMode.Flurry: return VMM_IconTexture.VMM_Flurry_Icon;
                case VMM_MeleeMode.Guard: return VMM_IconTexture.VMM_Guard_Icon;
                default: return VMM_IconTexture.VMM_Default_Icon;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (parent is not Pawn pawn) return;
            // 30 ticks 执行一次，CompTickRare 大概4秒间隔太长，这里还是用CompTick + 手动控制
            if (!parent.IsHashIntervalTick(_compTickInterval)) return;
            // 战斗状态才执行决策
            if (pawn.mindState.enemyTarget == null ||
                pawn.CurJob?.def != RimWorld.JobDefOf.AttackMelee ||
                pawn.stances.curStance is not Stance_Busy) return;
            if (curEnableAutoSelection && MeleeModeDB.Settings.enableAutoSelectionForPlayer)
                TriggerEvaluation();
        }

        // 自动切换是否在冷却
        private bool IsInCooldown()
        {
            int currentTick = GenTicks.TicksGame;
            return currentTick - lastSwitchTick < _switchModeCoolDown;
        }

        // 决策层入口
        public void TriggerEvaluation()
        {
            if (parent is not Pawn pawn) return;
            // 基础有效性检查
            if (!pawn.Spawned || pawn.Dead || pawn.Downed || pawn.Map == null)
                return;
            // 冷却检查
            if (IsInCooldown())
                return;
            VMM_MeleeMode desiredMode = EvaluateAndGetDesiredMode(pawn);
            ApplyModeSwitch(desiredMode);
        }

        // 决策方法，返回需要切换至的 VMM_MeleeMode
        public VMM_MeleeMode EvaluateAndGetDesiredMode(Pawn pawn)
        {
            VMM_MeleeMode? ModeByHardRules = ApplyHardRules(pawn);
            if (ModeByHardRules.HasValue)
            {
                return ModeByHardRules.Value;
            }

            return CalculateScoringMode(pawn);
        }

        // 硬规则层
        private static VMM_MeleeMode? ApplyHardRules(Pawn pawn)
        {
            float selfMissing = GetHealthMissingPercent(pawn);

            // P0: 生死存亡 -> 强制防御
            if (selfMissing >= 0.60f || GetEnemiesAttackingMe(pawn) >= 3)
                return VMM_MeleeMode.Guard;

            // P1: 收割残血敌人 -> 强制强攻
            Pawn? target = GetCurrentTarget(pawn);
            if (target != null)
            {
                float targetMissing = GetHealthMissingPercent(target);
                bool isDownedOrVeryWeak = target.Downed || targetMissing >= 0.70f;
                if (isDownedOrVeryWeak && GetEnemiesInMeleeRange(pawn) <= 2)
                    return VMM_MeleeMode.Aggressive;
            }

            return null; // 未命中硬规则，继续走评分层
        }

        // 加权评分层
        private static VMM_MeleeMode CalculateScoringMode(Pawn pawn)
        {
            Pawn? target = GetCurrentTarget(pawn);
            // TODO：需要再改进一下，比如判断一下目标的护甲或威胁度
            float targetMissing = target != null ? GetHealthMissingPercent(target) : 0f;
            float selfMissing = GetHealthMissingPercent(pawn);

            int enemiesInRange = GetEnemiesInMeleeRange(pawn);
            int enemiesAttackingMe = GetEnemiesAttackingMe(pawn);
            int alliesInRange = GetAlliesInMeleeRange(pawn);
            float avgEnemyDPS = GetAverageEnemyMeleeDPS(pawn);
            float targetArmor = GetCurrentTargetArmor(pawn);

            // TODO: 加权系数还需要再调整下
            // 强攻模式得分
            float aggressiveScore = (targetMissing * 2.2f)
                                    + (alliesInRange * 18f)
                                    - (enemiesAttackingMe * 15f)
                                    - (selfMissing * 0.9f)
                                    - (avgEnemyDPS * 14f)
                                    + (targetArmor * 24f);

            // 迅捷模式得分
            float flurryScore = (enemiesInRange * 14f)
                                + (enemiesAttackingMe * 9f)
                                + (alliesInRange * 22f)
                                - (targetMissing * 0.7f)
                                + (avgEnemyDPS * 16f)
                                - (targetArmor * 11f);

            // 默认得分（带轻微随机抖动）
            float defaultScore = 38f + Mathf.Sin(GenTicks.TicksGame / 25f) * 9f;

            // 取最高分
            if (aggressiveScore >= flurryScore && aggressiveScore >= defaultScore)
                return VMM_MeleeMode.Aggressive;
            if (flurryScore >= defaultScore)
                return VMM_MeleeMode.Flurry;

            return VMM_MeleeMode.Default;
        }

        private void ApplyModeSwitch(VMM_MeleeMode newMode)
        {
            if (parent is not Pawn pawn) return;

            if (curMode == newMode)
                return;
            curMode = newMode;

            lastSwitchTick = GenTicks.TicksGame;

            // TODO：看看实际效果，文字特效要不要保留？
            if (pawn.Drafted && pawn.IsColonist)
            {
                MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, newMode.ToString(), Color.yellow);
            }
        }

        // pawn 当前失去的生命值百分比
        private static float GetHealthMissingPercent(Pawn p)
        {
            return 1f - p.health.summaryHealth.SummaryHealthPercent;
        }

        // pawn 当前目标
        private static Pawn? GetCurrentTarget(Pawn pawn)
        {
            // 优先使用 AI 锁定的目标
            if (pawn.mindState.enemyTarget is Pawn targetPawn)
                return targetPawn;

            // Fallback 到当前 Job 的目标
            if (pawn.CurJob?.targetA.Thing is Pawn jobTarget)
                return jobTarget;

            return null;
        }

        // 近战攻击范围内的敌人数量
        private static int GetEnemiesInMeleeRange(Pawn pawn)
        {
            int count = 0;
            foreach (Thing t in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, 1.9f, true))
            {
                if (t is Pawn other && other.HostileTo(pawn) && !other.Downed)
                    count++;
            }

            return count;
        }

        // 正在攻击当前 pawn 的敌人数量
        private static int GetEnemiesAttackingMe(Pawn pawn)
        {
            int count = 0;
            foreach (Pawn other in pawn.Map.mapPawns.AllPawnsSpawned)
            {
                if (other.HostileTo(pawn) &&
                    other.CurJob?.def == JobDefOf.AttackMelee &&
                    other.CurJob.targetA.Thing == pawn)
                {
                    count++;
                }
            }

            return count;
        }

        // 近战范围内的友方数量
        private static int GetAlliesInMeleeRange(Pawn pawn)
        {
            int count = 0;
            foreach (Thing t in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, 1.9f, true))
            {
                if (t is Pawn other && other.Faction == pawn.Faction && !other.HostileTo(pawn))
                    count++;
            }

            return count;
        }

        // 计算近战范围内所有敌人的平均近战DPS
        private static float GetAverageEnemyMeleeDPS(Pawn pawn)
        {
            float totalDPS = 0f;
            int count = 0;

            foreach (Thing t in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, 1.9f, true))
            {
                if (t is Pawn other && other.HostileTo(pawn) && !other.Downed)
                {
                    totalDPS += other.GetStatValue(StatDefOf.MeleeDPS);
                    count++;
                }
            }

            return count > 0 ? totalDPS / count : 0f;
        }

        // 只计算当前攻击目标的护甲
        private static float GetCurrentTargetArmor(Pawn pawn)
        {
            var target = GetCurrentTarget(pawn);
            if (target == null) return 0f;
            // TODO: 之后最好改成根据 pawn 所持武器的主要伤害类型判断
            // 锐器护甲
            float sharp = target.GetStatValue(StatDefOf.ArmorRating_Sharp);
            if (sharp > 0.01f) return sharp;

            // 钝器护甲
            return target.GetStatValue(StatDefOf.ArmorRating_Blunt);
        }
    }
}