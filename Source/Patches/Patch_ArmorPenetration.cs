using HarmonyLib;
using Verse;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Patches
{
    [HarmonyPatch]
    public static class Patch_ArmorPenetration
    {
        //Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource
        [HarmonyPatch(typeof(VerbProperties))]
        [HarmonyPatch(nameof(VerbProperties.AdjustedArmorPenetration),new Type[] {typeof(Tool), typeof(Pawn), typeof(Thing), typeof(HediffComp_VerbGiver) })]
        [HarmonyPostfix]
        static void Patch_ArmorPenetration_1(VerbProperties __instance, Pawn attacker, ref float __result)
        {
            if(attacker == null) return;
            if (!__instance.IsMeleeAttack) return;
            VMM_MeleeMode meleeMode = attacker.VMM_GetMeleeMode();

            __result *= MeleeModeDB.GetMeleeArmorPenetration(meleeMode);
        }
        //Tool tool, Pawn attacker, ThingDef equipment, ThingDef equipmentStuff, HediffComp_VerbGiver hediffCompSource
        [HarmonyPatch(typeof(VerbProperties))]
        [HarmonyPatch(nameof(VerbProperties.AdjustedArmorPenetration), new Type[] { typeof(Tool), typeof(Pawn), typeof(ThingDef), typeof(ThingDef), typeof(HediffComp_VerbGiver) })]
        [HarmonyPostfix]
        static void Patch_ArmorPenetration_2(VerbProperties __instance, Pawn attacker, ref float __result)
        {
            if (attacker == null) return;
            if (!__instance.IsMeleeAttack) return;
            VMM_MeleeMode meleeMode = attacker.VMM_GetMeleeMode();

            __result *= MeleeModeDB.GetMeleeArmorPenetration(meleeMode);

        }
    }
}
