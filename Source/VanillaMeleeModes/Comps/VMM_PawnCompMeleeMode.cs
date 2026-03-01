using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Utilities;

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
                (pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled || pawn.IsColonySubhumanPlayerControlled) &&
                pawn.Drafted
                ) {
                yield return new Command_Action
                {
                    icon = GetIconFor(curMode),
                    defaultLabel = Utils.GetMeleeModeLabelFor(curMode),
                    defaultDesc = "VMM_SwitchGizmoDesc".Translate(),
                    action = () =>
                    {
                        curMode = (VMM_MeleeMode)(((int)curMode + 1) % 4);
                        SoundDefOf.Tick_High.PlayOneShotOnCamera();
                    }
                };
                
            }
        }

        private Texture2D GetIconFor(VMM_MeleeMode mode)
        {
            switch (mode) { 
                case VMM_MeleeMode.Default: return VMM_IconTexture.VMM_Default_Icon;
                case VMM_MeleeMode.Aggressive: return VMM_IconTexture.VMM_Aggressive_Icon;
                case VMM_MeleeMode.Flurry: return VMM_IconTexture.VMM_Flurry_Icon;
                case VMM_MeleeMode.Guard: return VMM_IconTexture.VMM_Guard_Icon;
                default: return VMM_IconTexture.VMM_Default_Icon;
            }
        }
    }
}
