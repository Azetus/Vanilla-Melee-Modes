using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VMM_VanillaMeleeModes.Hediffs;
using VMM_VanillaMeleeModes.Stat;

namespace VMM_VanillaMeleeModes.Patches
{
    [HarmonyPatch(typeof(Thing), nameof(Thing.TakeDamage))]
    public static class Patch_Thing_TakeDamage
    {
        static void Prefix(Thing __instance, ref DamageInfo dinfo)
        {
            if (__instance is not Pawn defender)
                return;

            if (dinfo.Instigator is not Pawn attacker)
                return;

            if (dinfo.Def.isRanged)
                return;
            
            if(!CanParry(defender))
                return;

            float parryChance = defender.GetStatValue(VMM_StatDefOf.VMM_MeleeParryChance);
            float parryDamageReduction = defender.GetStatValue(VMM_StatDefOf.VMM_MeleeParryDamageReduction);
            float counterChance = defender.GetStatValue(VMM_StatDefOf.VMM_MeleeCounterChance);

            if (Rand.Chance(parryChance))
            {
                float newAmount = dinfo.Amount * (MathF.Max(0f, 1f - parryDamageReduction));

                MoteMaker.ThrowText(
                    defender.DrawPos,
                    defender.Map,
                    "Parry!",
                    3f);
                FleckMaker.ThrowMicroSparks(
                    defender.DrawPos,
                    defender.Map);

                SoundDefOf.MetalHitImportant.PlayOneShot(
                    new TargetInfo(defender.Position, defender.Map)
                    );

                dinfo.SetAmount(newAmount);

                if (Rand.Chance(counterChance))
                {
                    TryCounter(defender, attacker);
                }
            }
        }

        private static void TryCounter(Pawn defender, Pawn attacker)
        {
            if (defender == null || attacker == null || !defender.Spawned || !attacker.Spawned)
                return;

            Verb? verb = defender.meleeVerbs?.TryGetMeleeVerb(attacker);

            if (verb == null || verb is not Verb_MeleeAttack meleeVerb)
                return;

            if(!CanCounter(defender, attacker, meleeVerb)) 
                return;  

            if (defender.stances != null)
            {
                if(defender.stances.curStance is Stance_Cooldown)
                {
                    defender.stances.CancelBusyStanceHard();
                }
                else
                {
                    defender.stances.CancelBusyStanceSoft();
                }

                bool attackSuccess = verb.TryStartCastOn(attacker, canHitNonTargetPawns: false, nonInterruptingSelfCast: true);
                if (attackSuccess)
                {
                    defender.health.AddHediff(VMM_HediffDefOf.VMM_CounterAttackCooldown);
                    MoteMaker.ThrowText(defender.DrawPos,
                        defender.Map,
                        "Counter!",
                        Color.red,
                        3f);
                }
            }

        }

        private static bool CanParry(Pawn defender)
        {
            if(defender == null)
                return false;
            // 排除死亡或倒下
            if (defender.Dead || defender.Downed)
                return false;
            // 只有Humanlike和Mechnoid能格挡
            if (defender.RaceProps == null || !(defender.RaceProps.Humanlike || defender.RaceProps.IsMechanoid))
                return false;
            // 必须有操作能力
            if(!defender.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                return false;
            // 无法移动
            if (IsImmobile(defender))
                return false;
            // 排除被眩晕状态
            if (defender.stances?.stunner?.Stunned ?? false)
                return false;
            // 排除社交斗殴
            if(defender.MentalStateDef == MentalStateDefOf.SocialFighting)
                return false;

            return true;
        }

        private static bool CanCounter(Pawn defender, Pawn attacker, Verb_MeleeAttack meleeVerb)
        {
            if (defender == null || attacker == null || meleeVerb == null)
                return false;
            // 排除死亡或倒下
            if (defender.Dead || defender.Downed)
                return false;
            // 对面倒下或死亡就不用反击了
            if (attacker.Dead || attacker.Downed)
                return false;
            // 只有Humanlike和Mechnoid能格挡
            if (defender.RaceProps == null || !(defender.RaceProps.Humanlike || defender.RaceProps.IsMechanoid))
                return false;
            // 必须有操作能力
            if (!defender.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                return false;
            // 无暴力能力无法反击
            if (defender.WorkTagIsDisabled(WorkTags.Violent))
                return false;
            // 无法移动
            if (IsImmobile(defender))
                return false;
            // 排除被眩晕状态
            if (defender.stances?.stunner?.Stunned ?? false)
                return false;
            // 反击能力冷却中
            if (defender.health?.hediffSet?.HasHediff(VMM_HediffDefOf.VMM_CounterAttackCooldown) ?? true)
                return false;
            // 排除社交斗殴
            if (defender.MentalStateDef == MentalStateDefOf.SocialFighting)
                return false;
            // 是否能打中目标
            if(!meleeVerb.CanHitTarget(attacker))
                return false;


            return true;
        }

        private static bool IsImmobile(Pawn pawn)
        {
            if (!pawn.Downed)
            {
                return pawn.GetPosture() != PawnPosture.Standing;
            }
            return true;
        }
    }
}
