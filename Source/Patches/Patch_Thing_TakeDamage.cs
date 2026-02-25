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
            if (defender.RaceProps == null || !defender.RaceProps.Humanlike)
                return;
            if (defender.Dead || defender.Downed)
                return;
            if (defender.stances?.stunner?.Stunned ?? false)
                return;

            if (defender.health?.hediffSet?.HasHediff(VMM_HediffDefOf.VMM_CounterAttackCooldown) ?? true)
                return;

            //Job counterJob = JobMaker.MakeJob(JobDefOf.AttackMelee, attacker);

            //counterJob.checkOverrideOnExpire = false;
            //counterJob.expiryInterval = 30;

            //defender.jobs.StartJob(
            //    counterJob,
            //    JobCondition.InterruptForced,
            //    null,
            //    resumeCurJobAfterwards: true
            //);



            Verb? verb = defender.meleeVerbs?.TryGetMeleeVerb(attacker);
            if (verb == null || !verb.CanHitTarget(attacker))
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




            //Pawn_MeleeVerbs? meleeVerbs = defender.meleeVerbs;
            //if (meleeVerbs != null && meleeVerbs is Pawn_MeleeVerbs meleeVerb && verb.CanHitTarget(attacker))
            //{

            //    defender.health.AddHediff(VMM_HediffDefOf.VMM_CounterAttackCooldown);

            //    bool attackSuccess = meleeVerbs.TryMeleeAttack(attacker);
            //}
        }
    }
}
