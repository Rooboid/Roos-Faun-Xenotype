﻿using RimWorld;
using Verse;
using Verse.AI;

namespace Roos_Faun_Xenotype
{
    public class ThoughtWorker_Myotonia : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!ModsConfig.BiotechActive)
            {
                return ThoughtState.Inactive;
            }
            if (p.genes == null || !p.genes.HasActiveGene(RBSF_DefOf.RBSF_Myotonia) || !ThoughtWorker_Myotonia.NearThreat(p))
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }

        public static bool NearThreat(Pawn pawn)
        {
            Map mapHeld = pawn.MapHeld;
            if (mapHeld == null)
            {
                return false;
            }
            IntVec3 positionHeld = pawn.PositionHeld;
            int num = GenRadial.NumCellsInRadius(fearRadius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[i];
                Pawn nearPawn = intVec.GetFirstPawn(mapHeld);
                bool isHostile = nearPawn != null && nearPawn.HostileTo(pawn) && !nearPawn.ThreatDisabled(nearPawn);
                bool isNearFire = intVec.ContainsStaticFire(mapHeld);
                if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && GenSight.LineOfSight(positionHeld, intVec, mapHeld, true, null, 0, 0) && (isHostile || isNearFire))
                {
                    return true;
                }
            }
            return false;
        }

        public const float fearRadius = 9.9f;
    }
}

