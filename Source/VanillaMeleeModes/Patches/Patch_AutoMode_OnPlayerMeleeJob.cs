using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using VMM_VanillaMeleeModes.Comps;

namespace VMM_VanillaMeleeModes.Patches
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
    public static class Patch_AutoMode_OnPlayerMeleeJob
    {
        // 仅玩家Pawn的AttackMelee Job：点击即触发评估
        // NPC不经过此轨——AI think tree的StartJob调用会被Faction过滤，
        // 所有NPC评估由CompTick轮询轨统一处理，避免冲突
        public static void Postfix(Job newJob, Pawn ___pawn)
        {
            if (newJob.def != JobDefOf.AttackMelee)
                return;
            if (___pawn.Faction != Faction.OfPlayer)
                return;

            var comp = ___pawn.TryGetComp<VMM_PawnCompMeleeMode>();
            if (comp == null)
                return;

            comp.OnPlayerStartedMeleeJob();
        }
    }
}