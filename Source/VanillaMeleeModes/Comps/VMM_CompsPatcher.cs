using Verse;

namespace VMM_VanillaMeleeModes.Comps
{
    [StaticConstructorOnStartup]
    public static class VMM_CompsPatcher
    {
        static VMM_CompsPatcher()
        {
            foreach (var def in DefDatabase<ThingDef>.AllDefs)
            {
                if(def.race != null)
                {
                    if(def.race.Humanlike || def.race.ToolUser)
                    {
                        if(def.comps == null)
                        {
                            def.comps = new List<CompProperties>();
                        }
                        if(!def.comps.Any(c => c is VMM_CompProperties_MeleeMode))
                        {
                            def.comps.Add(new VMM_CompProperties_MeleeMode());
                        }
                    }
                }
            }
        }
    }
}
