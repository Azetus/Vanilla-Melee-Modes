using RimWorld;
using UnityEngine;
using Verse;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Patch_CombatExtended.Utilities
{
    public static class CEEvaluator
    {
        // 评估常量（与原版保持一致）
        private const float EMERGENCY_HP_THRESHOLD = 0.3f;
        private const int EMERGENCY_THREAT_COUNT = 5;
        private const float THREAT_SEARCH_RADIUS = 1.9f;
        private const float HYSTERESIS_MULTIPLIER = 1.15f;

        // CE 护甲归一化因子：CE mm RHA / 10 → 原版 0~2 尺度
        private const float ARMOR_NORMALIZATION_FACTOR = 10f;

        // 主评估入口
        public static VMM_MeleeMode Evaluate(Pawn pawn, Thing? target,
            VMM_MeleeMode currentMode)
        {
            // 采集战场上下文
            int enemyCount = CountNearbyThreats(pawn);
            int allyCount = CountNearbyAllies(pawn);

            // 层级1：紧急规则（与原版完全相同）
            if (EvaluateTier1_Emergency(pawn, target, enemyCount, allyCount,
                    out VMM_MeleeMode result))
                return result;

            // 层级2：CE 加权评分
            return EvaluateTier2_CE_Scoring(pawn, target, enemyCount, allyCount,
                currentMode);
        }

        // 层级1：紧急规则（Guard由硬规则触发，不参与评分）
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
            // 被围或孤立被围
            if (enemyCount >= EMERGENCY_THREAT_COUNT
                || (enemyCount >= 3 && allyCount == 0))
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

        // 层级2：CE 三维度加权评分
        private static VMM_MeleeMode EvaluateTier2_CE_Scoring(Pawn pawn,
            Thing? target, int enemyCount, int allyCount,
            VMM_MeleeMode currentMode)
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

                // CE 护甲归一化：mm RHA → 原版 0~2 尺度
                float sharp = tp.GetStatValue(StatDefOf.ArmorRating_Sharp);
                float blunt = tp.GetStatValue(StatDefOf.ArmorRating_Blunt);
                targetArmor = Mathf.Max(sharp, blunt) / ARMOR_NORMALIZATION_FACTOR;
            }
            float targetMissingHP = 1f - targetHP;

            // 进攻分：累加上下文加成（与原版完全相同的公式）
            float aggScore = 1.0f
                + 0.3f                               // 模式基础进攻优势
                + targetMissingHP * 1.5f           // 收割冲动
                + allyCount * 0.25f                // 有队友时更敢输出
                - (enemyCount - 1) * 0.4f          // 多目标输出受限
                + Mathf.Min(targetArmor, 1.5f) * 0.6f;  // 高甲目标需穿甲

            float flurryScore = 1.0f
                + (meleeSkill / 20f) * 1.5f        // 高手技能兑现
                + Mathf.Min(targetDodge / 0.3f, 1f) * 1.0f  // 克制高闪避
                - Mathf.Max(enemyCount - 1, 0) * 0.4f  // 多目标连击无效
                - targetMissingHP * 1.0f           // 残血目标浪费连击
                - Mathf.Min(targetArmor, 1.5f) * 0.7f;  // 高甲弹刀

            float defaultScore = 1.0f;              // 锚点

            // 防御分（仅闪避维度，格挡由规则触发）
            float aggDef = 0.85f * (1f + (enemyCount - 1) * 0.3f);
            float flurryDef = 1.0f * (1f + (enemyCount - 1) * 0.3f);
            float defaultDef = 1.0f * (1f + (enemyCount - 1) * 0.3f);

            // CE 专属维度：格挡 + 暴击（暴击替代原版反击）
            float aggSpecial = MeleeModeDB_CE.GetMeleeParryChanceFactor_CE(
                    VMM_MeleeMode.Aggressive) * 0.3f
                + MeleeModeDB_CE.GetMeleeCritChanceFactor_CE(
                    VMM_MeleeMode.Aggressive) * 0.5f;
            float flurrySpecial = MeleeModeDB_CE.GetMeleeParryChanceFactor_CE(
                    VMM_MeleeMode.Flurry) * 0.3f
                + MeleeModeDB_CE.GetMeleeCritChanceFactor_CE(
                    VMM_MeleeMode.Flurry) * 0.5f;
            float defaultSpecial = MeleeModeDB_CE.GetMeleeParryChanceFactor_CE(
                    VMM_MeleeMode.Default) * 0.3f
                + MeleeModeDB_CE.GetMeleeCritChanceFactor_CE(
                    VMM_MeleeMode.Default) * 0.5f;

            // 加总三维度
            aggScore += aggDef + aggSpecial;
            flurryScore += flurryDef + flurrySpecial;
            defaultScore += defaultDef + defaultSpecial;

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
            var hostileTargets = pawn.Map.attackTargetsCache
                .TargetsHostileToFaction(pawn.Faction);
            foreach (var target in hostileTargets)
                if (target.Thing is Pawn other && !other.Dead && !other.Downed
                    && other.Position.InHorDistOf(pawn.Position,
                        THREAT_SEARCH_RADIUS))
                    count++;
            return count;
        }

        // 采集半径内友方Pawn数量（排除自身及死亡/倒地）
        private static int CountNearbyAllies(Pawn pawn)
        {
            if (pawn.Map == null) return 0;
            int count = 0;
            foreach (Pawn other in pawn.Map.mapPawns
                .SpawnedPawnsInFaction(pawn.Faction))
                if (other != pawn && !other.Dead && !other.Downed
                    && other.Position.InHorDistOf(pawn.Position,
                        THREAT_SEARCH_RADIUS))
                    count++;
            return count;
        }
    }
}