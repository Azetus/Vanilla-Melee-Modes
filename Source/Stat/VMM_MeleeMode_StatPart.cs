using RimWorld;
using Verse;
using VMM_VanillaMeleeModes.Settings;
using VMM_VanillaMeleeModes.Comps;
using VMM_VanillaMeleeModes.Utilities;

namespace VMM_VanillaMeleeModes.Stat
{
    public abstract class VMM_MeleeMode_StatPart : StatPart
    {
        protected abstract float GetFactor(VMM_MeleeMode mode);

        public override void TransformValue(StatRequest req, ref float val)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if(comp == null) return;
            val *= GetFactor(comp.curMode);
        }

        public override string? ExplanationPart(StatRequest req)
        {
            VMM_PawnCompMeleeMode? comp = TryGetComp(req);
            if (comp == null) return null;
            var factor = GetFactor(comp.curMode);

            return null;
        }

        private static VMM_PawnCompMeleeMode? TryGetComp(StatRequest req) {
            if(!req.HasThing) return null;
            if(req.Thing is not Pawn pawn) return null;

            return pawn.TryGetComp<VMM_PawnCompMeleeMode>();
        }


    }

    public class VMM_MeleeMode_MeleeHitChance_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeHitChance(mode);
        }
    }

    public class VMM_MeleeMode_MeleeDodgeChance_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeDodgeChance(mode);
        }
    }

    public class VMM_MeleeMode_MeleeDamageFactor_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeDamageFactor(mode);
        }
    }

    public class VMM_MeleeMode_MeleeCooldownFactor_FactorPart : VMM_MeleeMode_StatPart
    {
        protected override float GetFactor(VMM_MeleeMode mode)
        {
            return MeleeModeDB.GetMeleeCooldownFactor(mode);
        }
    }
}
