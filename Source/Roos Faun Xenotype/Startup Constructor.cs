using Verse;
using HarmonyLib;

namespace Roos_Faun_Xenotype
{
    [StaticConstructorOnStartup]
    public static class RBSF_Faun
    {
        static RBSF_Faun()
        {
            Harmony harmony = new Harmony("rimworld.mod.rooboid.faun");
            harmony.PatchAll();
            Log.Message("FAUN STATIC CONSTRUCTOR LOADED.");
        }
    }
}
