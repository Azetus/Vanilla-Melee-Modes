using RimWorld;
using Verse;
using Verse.Sound;
using VMM_VanillaMeleeModes.Settings;

namespace VMM_VanillaMeleeModes.Comps
{
    public class VMM_PawnCompMeleeMode : ThingComp
    {
        private VMM_MeleeMode mode = VMM_MeleeMode.Default;

        public VMM_MeleeMode curMode
        {
            get => mode;
            set => mode = value;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref mode, "VMM_meleeMode", VMM_MeleeMode.Default);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (parent is Pawn pawn &&
                (pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled) &&
                pawn.Drafted
                ) {
                yield return new Command_Action
                {
                    defaultLabel = "切换近战模式",
                    defaultDesc = "切换近战模式",
                    action = () =>
                    {
                        curMode = (VMM_MeleeMode)(((int)curMode + 1) % 4);
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                };
                
            }
        }
    }
}
