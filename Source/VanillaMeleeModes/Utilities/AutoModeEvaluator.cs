using RimWorld;
using VMM_VanillaMeleeModes.Settings;
using UnityEngine;
using Verse;

namespace VMM_VanillaMeleeModes.Utilities
{
    public static class AutoModeEvaluator
    {
        // 评估常量
        public const float EMERGENCY_HP_THRESHOLD = 0.3f;
        public const int EMERGENCY_THREAT_COUNT = 5;
        public const float THREAT_SEARCH_RADIUS = 1.9f;
        public const float HYSTERESIS_MULTIPLIER = 1.15f;

        // CE 替代评估器（CE 子 DLL 通过 RegisterCEEvaluator 挂载）
        private static Func<Pawn, Thing?, VMM_MeleeMode, VMM_MeleeMode>? _evaluator = null;

        public static void RegisterCEEvaluator(
            Func<Pawn, Thing?, VMM_MeleeMode, VMM_MeleeMode> evaluator)
        {
            _evaluator = evaluator;
        }

        // 是否处于近战战斗
        public static bool IsInMeleeCombat(Pawn pawn)
        {
            return pawn.mindState?.meleeThreat != null
                   || pawn.CurJobDef == JobDefOf.AttackMelee;
        }

        // 获取当前近战目标（优先meleeThreat）
        public static Thing? GetCombatTarget(Pawn pawn)
        {
            return (Thing?)pawn.mindState?.meleeThreat
                   ?? pawn.CurJob?.targetA.Thing;
        }

        // 冷却期内紧急Guard越级检查
        public static bool ShouldTriggerEmergencyGuard(Pawn pawn)
        {
            if (pawn.health?.summaryHealth.SummaryHealthPercent <= EMERGENCY_HP_THRESHOLD)
                return true;
            if (CountNearbyThreats(pawn) >= EMERGENCY_THREAT_COUNT)
                return true;
            return false;
        }

        // 主评估入口：紧急 -> 评分
        public static VMM_MeleeMode Evaluate(Pawn pawn, Thing? target,
            VMM_MeleeMode currentMode)
        {
            if (_evaluator != null)
                return _evaluator(pawn, target, currentMode);

            // 采集战场上下文
            int enemyCount = CountNearbyThreats(pawn);
            int allyCount = enemyCount >= 3 ? CountNearbyAllies(pawn) : 0;

            // 层级1：紧急规则
            if (EvaluateTier1_Emergency(pawn, target, enemyCount, allyCount, out VMM_MeleeMode result))
                return result;

            // 层级2：加权评分
            return EvaluateTier3_Scoring(pawn, target, enemyCount, allyCount, currentMode);
        }

        // 层级1：紧急规则（硬规则 -> Guard）
        private static bool EvaluateTier1_Emergency(Pawn pawn, Thing? target,
            int enemyCount, int allyCount, out VMM_MeleeMode result)
        {
            result = VMM_MeleeMode.Default;
            float hp = pawn.health?.summaryHealth.SummaryHealthPercent ?? 1f;

            // 濒死求生
            if (hp <= EMERGENCY_HP_THRESHOLD)
            {
                result = VMM_MeleeMode.Guard;
                return true;
            }

            // 被围或孤立被围
            if (enemyCount >= EMERGENCY_THREAT_COUNT || (enemyCount >= 3 && allyCount == 0))
            {
                result = VMM_MeleeMode.Guard;
                return true;
            }

            if (target is Pawn targetPawn)
            {
                // 安全收割倒地目标
                if (targetPawn.Downed && enemyCount <= 1)
                {
                    result = VMM_MeleeMode.Aggressive;
                    return true;
                }

                float targetHp = targetPawn.health?.summaryHealth.SummaryHealthPercent ?? 1f;
                // 强力收尾残血目标
                if (hp >= 0.8f && targetHp <= 0.2f)
                {
                    result = VMM_MeleeMode.Aggressive;
                    return true;
                }
            }

            return false;
        }


        // 层级2：三维度加权评分（Guard不参与评分，仅由规则触发）
        private static VMM_MeleeMode EvaluateTier3_Scoring(Pawn pawn, Thing? target,
            int enemyCount, int allyCount, VMM_MeleeMode currentMode)
        {
            // 采集评分输入因子
            float meleeSkill = pawn.skills?.GetSkill(SkillDefOf.Melee)?.Level ?? 0f;

            float targetDodge = 0f;
            float targetArmor = 0f;
            if (target is Pawn tp)
            {
                targetDodge = tp.GetStatValue(StatDefOf.MeleeDodgeChance);

                // 护甲：自然护甲 + 逐服饰取最大值
                targetArmor = Mathf.Max(
                    tp.GetStatValue(StatDefOf.ArmorRating_Sharp),
                    tp.GetStatValue(StatDefOf.ArmorRating_Blunt));
                if (tp.apparel != null)
                    foreach (var a in tp.apparel.WornApparel)
                        targetArmor = Mathf.Max(targetArmor,
                            a.GetStatValue(StatDefOf.ArmorRating_Sharp),
                            a.GetStatValue(StatDefOf.ArmorRating_Blunt));
            }

            // 各模式 raw DPS（命中×伤害×穿甲/冷却）
            float aggRawDPS = MeleeModeDB.GetMeleeHitChance(VMM_MeleeMode.Aggressive)
                              * MeleeModeDB.GetMeleeDamageFactor(VMM_MeleeMode.Aggressive)
                              * MeleeModeDB.GetMeleeArmorPenetration(VMM_MeleeMode.Aggressive)
                              / MeleeModeDB.GetMeleeCooldownFactor(VMM_MeleeMode.Aggressive);
            float flurryRawDPS = MeleeModeDB.GetMeleeHitChance(VMM_MeleeMode.Flurry)
                                 * MeleeModeDB.GetMeleeDamageFactor(VMM_MeleeMode.Flurry)
                                 * MeleeModeDB.GetMeleeArmorPenetration(VMM_MeleeMode.Flurry)
                                 / MeleeModeDB.GetMeleeCooldownFactor(VMM_MeleeMode.Flurry);

            // 进攻分：累加上下文加成
            float aggScore = (1.0f
                              + Mathf.Min(targetArmor, 1.5f) * 0.8f) // 高甲目标需穿甲
                             * aggRawDPS;

            float flurryScore = (1.0f
                                 + (meleeSkill / 20f) * 1.5f // 高手技能兑现
                                 + Mathf.Min(targetDodge / 0.3f, 1f) * 1.0f // 克制高闪避
                                 - Mathf.Min(targetArmor, 1.5f) * 0.9f) // 高甲弹刀
                                * flurryRawDPS;

            float defaultScore = 1.0f;
            // 防御分（仅闪避维度，格挡由规则触发）
            float aggDef = MeleeModeDB.GetMeleeDodgeChance(VMM_MeleeMode.Aggressive)
                           * (1f + (enemyCount - 1) * 0.3f);
            float flurryDef = MeleeModeDB.GetMeleeDodgeChance(VMM_MeleeMode.Flurry)
                              * (1f + (enemyCount - 1) * 0.3f);
            float defaultDef = 1f * (1f + (enemyCount - 1) * 0.3f);

            // 反击分（parryChance × counterChance乘积）
            float aggCtr = MeleeModeDB.GetMeleeParryChanceFactor(VMM_MeleeMode.Aggressive)
                           * MeleeModeDB.GetMeleeCounterChanceFactor(VMM_MeleeMode.Aggressive);
            float flurryCtr = MeleeModeDB.GetMeleeParryChanceFactor(VMM_MeleeMode.Flurry)
                              * MeleeModeDB.GetMeleeCounterChanceFactor(VMM_MeleeMode.Flurry);
            float defaultCtr = 1f * 1f;

            // 加总三维度
            aggScore += aggDef + aggCtr * 0.3f;
            flurryScore += flurryDef + flurryCtr * 0.3f;
            defaultScore += defaultDef + defaultCtr * 0.3f;

            // 迟滞加权：当前模式得分乘系数防振荡
            switch (currentMode)
            {
                case VMM_MeleeMode.Aggressive:
                    aggScore *= HYSTERESIS_MULTIPLIER;
                    break;
                case VMM_MeleeMode.Flurry:
                    flurryScore *= HYSTERESIS_MULTIPLIER;
                    break;
                default:
                    defaultScore *= HYSTERESIS_MULTIPLIER;
                    break;
            }

            // 取最高分
            if (aggScore >= flurryScore && aggScore >= defaultScore)
                return VMM_MeleeMode.Aggressive;
            if (flurryScore >= defaultScore)
                return VMM_MeleeMode.Flurry;
            return VMM_MeleeMode.Default;
        }

        // 采集半径内正在攻击当前pawn的敌对Pawn数量（排除死亡/倒地）
        private static int CountNearbyThreats(Pawn pawn)
        {
            if (pawn.Map == null) return 0;
            int count = 0;
            var hostileTargets = pawn.Map.attackTargetsCache.TargetsHostileToFaction(pawn.Faction);
            foreach (var target in hostileTargets)
            {
                if (target.Thing is not Pawn other
                    || other.health.State != PawnHealthState.Mobile
                    || other.mindState?.meleeThreat != pawn
                    || !other.Position.InHorDistOf(pawn.Position, THREAT_SEARCH_RADIUS))
                    continue;
                if (++count >= EMERGENCY_THREAT_COUNT)
                    return count;
            }

            return count;
        }

        // 采集半径内友方Pawn数量（排除自身及死亡/倒地）
        private static int CountNearbyAllies(Pawn pawn)
        {
            if (pawn.Map == null || pawn.Faction == null) return 0;
            int count = 0;
            foreach (Pawn other in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction))
                if (other != pawn && !other.Dead && !other.Downed
                    && other.Position.InHorDistOf(pawn.Position, THREAT_SEARCH_RADIUS))
                    count++;
            return count;
        }
    }
}