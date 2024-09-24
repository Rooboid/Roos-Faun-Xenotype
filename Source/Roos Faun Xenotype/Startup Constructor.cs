using HarmonyLib;
using Verse;

namespace Roos_Faun_Xenotype
{
    [StaticConstructorOnStartup]
    public static class RBSF_Faun
    {
        static RBSF_Faun()
        {
            Harmony harmony = new Harmony("rimworld.mod.rooboid.faun");
            harmony.PatchAll();
            Log.Message("Roos_Faun_Xenotype Mod Harmony Patches Loaded");
        }
    }
}
