using HarmonyLib;
using Verse;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Utilities;


namespace VMM_VanillaMeleeModes.Patches;

[HarmonyPatch(typeof(Verb))]
[HarmonyPatch(
    nameof(Verb.TryStartCastOn),
    new Type[] { typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(bool), typeof(bool), typeof(bool), typeof(bool) }
)]
public class Patch_Verb_TryStartCastOn
{
    static void Prefix(Verb __instance, LocalTargetInfo castTarg)
    {
        if (__instance.CasterPawn is not Pawn pawn) return;
        if (!castTarg.IsValid) return;
        if (__instance.verbProps == null) return;
        if (__instance.verbProps.IsMeleeAttack)
        {
            var setting = MeleeModeDB.Settings;

            bool enableForPlayer = setting.enableAutoSelectionForPlayer;
            bool enableForNPC = setting.enableAutoSelectionForNPC;

            var comp = pawn.TryGetComp<VMM_PawnCompMeleeMode>();


            if (pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled)
            {
                if (enableForPlayer)
                {
                    if (comp != null && comp.curEnableAutoSelection)
                        comp.TriggerEvaluation();
                }
            }

            if (!pawn.IsColonistPlayerControlled && !pawn.IsColonyMechPlayerControlled)
            {
                if (enableForNPC)
                {
                    if (comp != null && comp.curEnableAutoSelection)
                        comp.TriggerEvaluation();
                }
            }
        }
    }
}