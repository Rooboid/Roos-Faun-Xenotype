﻿using RimWorld;
using Verse;
using Verse.AI;

namespace Roos_Faun_Xenotype
{
    public class ThoughtWorker_Mechanophobia : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!ModsConfig.BiotechActive)
            {
                return ThoughtState.Inactive;
            }
            if (p.genes == null || !p.genes.HasActiveGene(RBSF_DefOf.RBSF_Mechanophobia) || !ThoughtWorker_Mechanophobia.NearScaryMech(p))
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }

        public static bool NearScaryMech(Pawn pawn)
        {
            Map mapHeld = pawn.MapHeld;
            if (mapHeld == null)
            {
                return false;
            }
            IntVec3 positionHeld = pawn.PositionHeld;
            int num = GenRadial.NumCellsInRadius(mechRadius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[i];
                if (!intVec.InBounds(mapHeld) || intVec.Fogged(mapHeld) || !GenSight.LineOfSight(positionHeld, intVec, mapHeld, true, null, 0, 0))
                {
                    continue;
                }
                Pawn nearPawn = intVec.GetFirstPawn(mapHeld);
                if (nearPawn != null && nearPawn.RaceProps?.FleshType == FleshTypeDefOf.Mechanoid)
                {
                    if (FaunSettings.mechanophobiaAffectsFriendly)
                    {
                        return true;
                    }
                    if (nearPawn.HostileTo(pawn))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public const float mechRadius = 9.9f;
    }

    public class MentalBreakWorker_MinorCatatonic : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            return pawn.Spawned && base.BreakCanOccur(pawn) && ThoughtWorker_Mechanophobia.NearScaryMech(pawn);
        }

        public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
        {
            pawn.health.AddHediff(RBSF_DefOf.RBSF_MinorCatatonicBreakdown, null, null, null);
            //base.TrySendLetter(pawn, "RBSF_LetterMinorCatatonicMentalBreak", reason);
            return true;
        }
    }
}

