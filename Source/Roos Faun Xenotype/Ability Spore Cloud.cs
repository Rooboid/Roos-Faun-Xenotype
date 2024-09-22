using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Roos_Faun_Xenotype
{
    internal class Class1
    {

        public class CompAbilityEffect_SporeCloud : CompAbilityEffect
        {

            //Main 'See Red' ability method - terrifies pawns in an area and enrages the user.
            public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
            {
                Pawn pawn = this.parent.pawn;
                IntVec3 position = pawn.Position;
                Map map = pawn.Map;
                float radius = 10;

                base.Apply(target, dest);

                //pawn.health.AddHediff(RBM_DefOf.HeDiffSeeRed);
                GenExplosion.DoExplosion(target.Cell, map, radius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
                SporeExplodeInArea(position, map, radius, pawn);
            }

            //Draws radius circle
            public override void DrawEffectPreview(LocalTargetInfo target)
            {
                var adjustedRadius = 8 * this.parent.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
                GenDraw.DrawRadiusRing(target.Cell, adjustedRadius);
            }

            //Allows AI to use ability only if 35% to pain threshold
            public override bool AICanTargetNow(LocalTargetInfo target)
            {
                float? currentPain = this.parent?.pawn?.health?.hediffSet?.PainTotal;
                float allowedPain = this.parent.pawn.GetStatValue(StatDefOf.PainShockThreshold) * 0.35f;

                if (currentPain > allowedPain)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool SporeExplodeInArea(IntVec3 position, Map map, float radius = 5, Pawn originPawn = null)
        {
            if (map == null) { return false; }
            List<Pawn> mapPawns = (List<Pawn>)map.mapPawns.AllPawnsSpawned;

            for (int i = 0; i < mapPawns.Count; i++)
            {
                bool isHumanlike = mapPawns[i].RaceProps.Humanlike;
                bool isInRange = position.InHorDistOf(mapPawns[i].Position, radius);

                if (isHumanlike && isInRange) //GTox mask check?
                {
                    //do ability
                }
            }
            return true;
        }
    }
}
