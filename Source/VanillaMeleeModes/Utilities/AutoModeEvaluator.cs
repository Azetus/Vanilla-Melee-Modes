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
        public const int EMERGENCY_THREAT_COUNT = 3;
        public const float THREAT_SEARCH_RADIUS = 1.9f;
        public const float HYSTERESIS_MULTIPLIER = 1.15f;

        // 是否处于近战战斗
        public static bool IsInMeleeCombat(Pawn pawn)
        {
            return pawn.mindState.meleeThreat != null
                || pawn.CurJobDef == JobDefOf.AttackMelee;
        }

        // 获取当前近战目标（优先meleeThreat）
        public static Thing? GetCombatTarget(Pawn pawn)
        {
            return (Thing)pawn.mindState.meleeThreat
                ?? pawn.CurJob?.targetA.Thing;
        }

        // 冷却期内紧急Guard越级检查
        public static bool ShouldTriggerEmergencyGuard(Pawn pawn)
        {
            if (pawn.health.summaryHealth.SummaryHealthPercent <= EMERGENCY_HP_THRESHOLD)
                return true;
            if (CountNearbyThreats(pawn) >= EMERGENCY_THREAT_COUNT)
                return true;
            return false;
        }

        // 主评估入口：紧急 -> 评分
        public static VMM_MeleeMode Evaluate(Pawn pawn, Thing? target,
            VMM_MeleeMode currentMode)
        {
            // 采集战场上下文
            int enemyCount = CountNearbyThreats(pawn);
            int allyCount = CountNearbyAllies(pawn);

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
            float hp = pawn.health.summaryHealth.SummaryHealthPercent;

            // 濒死求生
            if (hp <= EMERGENCY_HP_THRESHOLD)
            {
                result = VMM_MeleeMode.Guard;
                return true;
            }
            // TODO: 判断条件可以改大一点
            // 被围或孤立被围
            if (enemyCount >= EMERGENCY_THREAT_COUNT || (enemyCount >= 2 && allyCount == 0))
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
                float targetHp = targetPawn.health.summaryHealth.SummaryHealthPercent;
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
            float selfHP = pawn.health.summaryHealth.SummaryHealthPercent;
            float meleeSkill = pawn.skills.GetSkill(SkillDefOf.Melee)?.Level ?? 0f;

            float targetHP = 1f;
            float targetDodge = 0f;
            float targetArmor = 0f;
            if (target is Pawn tp)
            {
                targetHP = tp.health.summaryHealth.SummaryHealthPercent;
                targetDodge = tp.GetStatValue(StatDefOf.MeleeDodgeChance);
                float sharp = tp.GetStatValue(StatDefOf.ArmorRating_Sharp);
                float blunt = tp.GetStatValue(StatDefOf.ArmorRating_Blunt);
                targetArmor = sharp > blunt ? sharp : blunt;
            }
            float targetMissingHP = 1f - targetHP;

            // 进攻分：累加上下文加成
            float aggScore = 1.0f
                + (selfHP - 0.5f) * 2.0f           // 血量优势
                + targetMissingHP * 1.5f           // 收割冲动
                + allyCount * 0.25f                // 有队友时更敢输出
                - (enemyCount - 1) * 0.4f          // 多目标输出受限
                + Mathf.Min(targetArmor, 1.5f) * 0.6f;  // 高甲目标需穿甲

            float flurryScore = 1.0f
                + (meleeSkill / 20f) * 1.2f        // 高手技能兑现
                + Mathf.Min(targetDodge / 0.3f, 1f) * 1.0f  // 克制高闪避
                + (enemyCount == 1 ? 0.5f : 0f)    // 单挑加成
                - Mathf.Max(enemyCount - 1, 0) * 0.4f  // 多目标连击无效
                - targetMissingHP * 1.0f           // 残血目标浪费连击
                - Mathf.Min(targetArmor, 1.5f) * 0.7f;  // 高甲弹刀

            float defaultScore = 1.0f;              // 锚点

            // 防御分（仅闪避维度，格挡由规则触发）
            float aggDef = 0.85f * (1f + (enemyCount - 1) * 0.3f);
            float flurryDef = 1.0f * (1f + (enemyCount - 1) * 0.3f);
            float defaultDef = 1.0f * (1f + (enemyCount - 1) * 0.3f);

            // 反击分（parryChance × counterChance乘积）
            float aggCtr = 0.90f * 1.50f;
            float flurryCtr = 0.80f * 0.60f;
            float defaultCtr = 1.0f * 1.0f;

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

        // 采集半径内敌对Pawn数量（排除死亡/倒地）
        private static int CountNearbyThreats(Pawn pawn)
        {
            if (pawn.Map == null) return 0;
            int count = 0;
            var hostileTargets = pawn.Map.attackTargetsCache.TargetsHostileToFaction(pawn.Faction);
            foreach (var target in hostileTargets)
                if (target.Thing is Pawn other && !other.Dead && !other.Downed
                    && other.Position.InHorDistOf(pawn.Position, THREAT_SEARCH_RADIUS))
                    count++;
            return count;
        }

        // 采集半径内友方Pawn数量（排除自身及死亡/倒地）
        private static int CountNearbyAllies(Pawn pawn)
        {
            if (pawn.Map == null) return 0;
            int count = 0;
            foreach (Pawn other in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction))
                if (other != pawn && !other.Dead && !other.Downed
                    && other.Position.InHorDistOf(pawn.Position, THREAT_SEARCH_RADIUS))
                    count++;
            return count;
        }
    }
}
