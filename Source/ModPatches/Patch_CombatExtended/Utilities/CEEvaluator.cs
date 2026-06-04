using CombatExtended;
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
            int allyCount = enemyCount >= 3 ? CountNearbyAllies(pawn) : 0;

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
            float meleeSkill = pawn.skills.GetSkill(SkillDefOf.Melee)?.Level ?? 0f;

            float targetDodge = 0f;
            float targetArmor = 0f;
            float armorWeight = 1.0f;
            if (target is Pawn tp)
            {
                targetDodge = tp.GetStatValue(StatDefOf.MeleeDodgeChance);

                if (tp.apparel != null)
                {
                    // 反推两种基础穿甲
                    var verb = pawn.meleeVerbs.TryGetMeleeVerb(target);
                    var verbCE = verb as Verb_MeleeAttackCE;
                    float rawSharpAP = verbCE?.ArmorPenetrationSharp ?? 0f;
                    float rawBluntAP = verbCE?.ArmorPenetrationBlunt ?? 0f;
                    float currentFactor = MeleeModeDB_CE.GetMeleeArmorPenetration_CE(
                        currentMode);
                    float baseSharpAP = currentFactor > 0.01f
                        ? rawSharpAP / currentFactor : rawSharpAP;
                    float baseBluntAP = currentFactor > 0.01f
                        ? rawBluntAP / currentFactor : rawBluntAP;

                    // 武器主导：取 AP 更高的维度，仅读该维度的护甲
                    bool useSharp = baseSharpAP >= baseBluntAP;
                    StatDef armorStat = useSharp
                        ? StatDefOf.ArmorRating_Sharp
                        : StatDefOf.ArmorRating_Blunt;
                    float baseAP = useSharp ? baseSharpAP : baseBluntAP;

                    float rawArmor = 0f;
                    foreach (var a in tp.apparel.WornApparel)
                        rawArmor = Mathf.Max(rawArmor, a.GetStatValue(armorStat));
                    // 裸体目标 targetArmor 保持 0 → 护甲加减分消失，正确行为
                    targetArmor = rawArmor / ARMOR_NORMALIZATION_FACTOR;

                    if (rawArmor > 0.01f)
                    {
                        float aggAP = baseAP
                            * MeleeModeDB_CE.GetMeleeArmorPenetration_CE(
                                VMM_MeleeMode.Aggressive);
                        float flurryAP = baseAP
                            * MeleeModeDB_CE.GetMeleeArmorPenetration_CE(
                                VMM_MeleeMode.Flurry);

                        if (aggAP >= rawArmor && flurryAP < rawArmor)
                            armorWeight = 3.0f;
                        else if (flurryAP >= rawArmor)
                            armorWeight = 0.5f;
                        else
                            armorWeight = 1.0f;
                    }
                }
            }

            // 各模式 raw DPS（命中×伤害×穿甲/冷却）
            float aggRawDPS = MeleeModeDB_CE.GetMeleeHitChance_CE(VMM_MeleeMode.Aggressive)
                            * MeleeModeDB_CE.GetMeleeDamageFactor_CE(VMM_MeleeMode.Aggressive)
                            * MeleeModeDB_CE.GetMeleeArmorPenetration_CE(VMM_MeleeMode.Aggressive)
                            / MeleeModeDB_CE.GetMeleeCooldownFactor_CE(VMM_MeleeMode.Aggressive);
            float flurryRawDPS = MeleeModeDB_CE.GetMeleeHitChance_CE(VMM_MeleeMode.Flurry)
                              * MeleeModeDB_CE.GetMeleeDamageFactor_CE(VMM_MeleeMode.Flurry)
                              * MeleeModeDB_CE.GetMeleeArmorPenetration_CE(VMM_MeleeMode.Flurry)
                              / MeleeModeDB_CE.GetMeleeCooldownFactor_CE(VMM_MeleeMode.Flurry);

            // 进攻分：累加上下文加成
            float aggScore = (1.0f
                + Mathf.Min(targetArmor, 1.5f) * 0.8f * armorWeight)  // 高甲目标需穿甲
                * aggRawDPS;

            float flurryScore = (1.0f
                + (meleeSkill / 20f) * 1.5f        // 高手技能兑现
                + Mathf.Min(targetDodge / 0.3f, 1f) * 1.0f  // 克制高闪避
                - Mathf.Min(targetArmor, 1.5f) * 0.9f * armorWeight)  // 高甲弹刀
                * flurryRawDPS;

            float defaultScore = 1.0f;
            // 防御分（仅闪避维度，格挡由规则触发）
            float aggDef = MeleeModeDB_CE.GetMeleeDodgeChance_CE(VMM_MeleeMode.Aggressive)
                * (1f + (enemyCount - 1) * 0.3f);
            float flurryDef = MeleeModeDB_CE.GetMeleeDodgeChance_CE(VMM_MeleeMode.Flurry)
                * (1f + (enemyCount - 1) * 0.3f);
            float defaultDef = 1f * (1f + (enemyCount - 1) * 0.3f);

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
            {
                if (target.Thing is not Pawn other
                    || other.health.State != PawnHealthState.Mobile
                    || other.mindState.meleeThreat != pawn
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