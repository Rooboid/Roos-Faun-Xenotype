using RimWorld;
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
            if (p.genes == null || !p.genes.HasActiveGene(RBSF_DefOf.RBSF_Mechanophobia) || !ThoughtWorker_Mechanophobia.NearMech(p))
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }

        public static bool NearMech(Pawn pawn)
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
                Pawn nearPawn = intVec.GetFirstPawn(mapHeld);
                if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && GenSight.LineOfSight(positionHeld, intVec, mapHeld, true, null, 0, 0) && nearPawn != null && nearPawn.RaceProps?.FleshType == FleshTypeDefOf.Mechanoid)
                {
                    return true;
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
            return pawn.Spawned && base.BreakCanOccur(pawn) && ( ThoughtWorker_Mechanophobia.NearMech(pawn) || ThoughtWorker_Myotonia.NearThreat(pawn) );
        }

        public override bool TryStart(Pawn pawn, string reason, bool causedByMood)
        {
            pawn.health.AddHediff(RBSF_DefOf.RBSF_MinorCatatonicBreakdown, null, null, null);
            base.TrySendLetter(pawn, "RBSF_LetterMinorCatatonicMentalBreak", reason);
            return true;
        }
    }
}

