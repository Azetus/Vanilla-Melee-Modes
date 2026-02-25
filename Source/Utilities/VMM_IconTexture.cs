using UnityEngine;
using Verse;

namespace VMM_VanillaMeleeModes.Utilities
{
    [StaticConstructorOnStartup]
    public static class VMM_IconTexture
    {
        public static readonly Texture2D VMM_Default_Icon = ContentFinder<Texture2D>.Get("VMM_Default_Icon");
        public static readonly Texture2D VMM_Aggressive_Icon = ContentFinder<Texture2D>.Get("VMM_Aggressive_Icon");
        public static readonly Texture2D VMM_Flurry_Icon = ContentFinder<Texture2D>.Get("VMM_Flurry_Icon");
        public static readonly Texture2D VMM_Guard_Icon = ContentFinder<Texture2D>.Get("VMM_Guard_Icon");
        public static readonly Texture2D VMM_Auto_Icon = ContentFinder<Texture2D>.Get("VMM_Auto_Icon");
    }
}
