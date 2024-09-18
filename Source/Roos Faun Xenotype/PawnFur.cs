using HarmonyLib;
using UnityEngine;
using Verse;

namespace Roos_Faun_Xenotype
{

    [HarmonyPatch(typeof(PawnRenderNode_Fur))]
    [HarmonyPatch("GraphicFor")]
    static class PawnRenderer_Prefix
    {
        static bool Prefix(Pawn pawn, ref Graphic __result)
        {
            if (pawn.genes.HasActiveGene(RBSF_DefOf.RBM_UnguligradeLegs))
            {
                //Mesh mesh = HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(___pawn).MeshAt(facing);
                Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(pawn.story.furDef.GetFurBodyGraphicPath(pawn), ShaderDatabase.CutoutSkinOverlay, Vector2.one, pawn.story.HairColor, Color.white, null, pawn.story.bodyType.bodyNakedGraphicPath);
                __result = graphic;
                //Material mat2 = graphic.MatAt(facing, null);
                //GenDraw.DrawMeshNowOrLater(mesh, shellLoc, quat, mat2, flags.FlagSet(PawnRenderFlags.DrawNow));
                return false;
            }
            return true;
        }
    }

}
