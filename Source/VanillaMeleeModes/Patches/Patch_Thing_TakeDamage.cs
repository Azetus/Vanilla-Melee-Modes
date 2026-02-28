using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VMM_VanillaMeleeModes.DefOfs;

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

            if (!IsValidMeleeAttack(dinfo, defender))
                return;
            
            if(!CanParry(defender))
                return;

            float parryChance = defender.GetStatValue(VMM_StatDefOf.VMM_MeleeParryChance);
            float parryDamageReduction = defender.GetStatValue(VMM_StatDefOf.VMM_MeleeParryDamageReduction);
            float counterChance = defender.GetStatValue(VMM_StatDefOf.VMM_MeleeCounterChance);
            
            float weaponFactor = GetWeaponParryFactor(defender);
            // 是否触发格挡
            if (Rand.Chance(parryChance * weaponFactor))
            {
                float newAmount = dinfo.Amount * (MathF.Max(0f, 1f - parryDamageReduction));

                MoteMaker.ThrowText(
                    defender.DrawPos,
                    defender.Map,
                    "VMM_MoteText_Parry".Translate(),
                    3f);
                FleckMaker.ThrowMicroSparks(
                    defender.DrawPos,
                    defender.Map);

                SoundDefOf.MetalHitImportant.PlayOneShot(
                    new TargetInfo(defender.Position, defender.Map)
                    );

                dinfo.SetAmount(newAmount);

                Find.BattleLog.Add(new BattleLogEntry_Event(
                    defender,
                    VMM_RulePackDefOf.VMM_Melee_Parried,
                    attacker));
                // 是否触发反击
                if (Rand.Chance(counterChance * weaponFactor))
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

                Find.BattleLog.Add(new BattleLogEntry_Event(
                    defender,
                    VMM_RulePackDefOf.VMM_Melee_CounterAttack,
                    attacker));


                bool attackSuccess = verb.TryStartCastOn(attacker, canHitNonTargetPawns: false, nonInterruptingSelfCast: true);

                if (attackSuccess)
                {
                    defender.health.AddHediff(VMM_HediffDefOf.VMM_CounterAttackCooldown);
                    MoteMaker.ThrowText(defender.DrawPos,
                        defender.Map,
                        "VMM_MoteText_Counter".Translate(),
                        Color.red,
                        3f);
                }
            }

        }

        // 是否持有武器
        private static float GetWeaponParryFactor(Pawn defender)
        {
            var weapon = defender?.equipment?.Primary;
            if (weapon == null) return 0f;

            if (weapon.def.IsMeleeWeapon)
                return 1f;

            if (weapon.def.IsRangedWeapon)
                return 0.5f;

            return 1f;
        }
        
        // 可格挡伤害来源
        private static bool IsValidMeleeAttack(DamageInfo dinfo, Pawn defender)
        {
            // 必须有攻击者
            if (dinfo.Instigator is not Pawn attacker)
                return false;

            // 不能是自己
            if (attacker == defender)
                return false;

            // 排除远程
            if (dinfo.Def.isRanged)
                return false;

            // 排除爆炸
            if (dinfo.Def.isExplosive)
                return false;

            // 排除处决
            if (dinfo.Def.execution)
                return false;
            
            // 排除非武器和肢体
            if (dinfo.Weapon == null && dinfo.WeaponBodyPartGroup == null)
                return false;

            return true;
        }

        private static bool CanParry(Pawn defender)
        {
            if(defender == null)
                return false;
            // 排除死亡或倒下
            if (defender.Dead || defender.Downed)
                return false;
            // 只有Humanlike和Mechanoid能格挡
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
            // 只有Humanlike和Mechanoid能格挡
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
