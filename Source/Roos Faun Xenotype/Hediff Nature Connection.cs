using RimWorld;
using System;
using Verse;

namespace Roos_Faun_Xenotype
{
    public class Comp_NatureConnectionHediff : HediffComp
    {
        //Scales severity with number of nearby plants
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (base.Pawn.IsHashIntervalTick(60))
            {
                var numPlants = CountNearPlants(parent.pawn);
                Log.Message("counted " + numPlants + " plants near " + parent.pawn.Name.ToStringShort);
                int severity;
                switch (numPlants)
                {
                    case int i when i < 15:
                        severity = 0;
                        break;
                    case int i when i < 30:
                        severity = 1;
                        break;
                    case int i when i < 40:
                        severity = 2;
                        break;
                    case int i when i < 50:
                        severity = 3;
                        break;
                    default:
                        severity = 4;
                        break;
                }
                this.parent.Severity = severity;
            }
        }

        public static int CountNearPlants(Pawn pawn)
        {
            Map mapHeld = pawn.MapHeld;
            if (mapHeld == null)
            {
                return -1;
            }
            IntVec3 positionHeld = pawn.PositionHeld;
            float plantRadius = 10.9f;
            int numCells = GenRadial.NumCellsInRadius(plantRadius);
            float plantTotalWeight = 0;
            for (int i = 0; i < numCells; i++)
            {
                //Scan radius around pawn for plants
                IntVec3 intVec = positionHeld + GenRadial.RadialPattern[i];
                Plant plant = intVec.GetPlant(mapHeld);

                //count plants
                if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && plant != null)
                {
                    if (plant.def.plant.IsTree && !plant.def.plant.isStump)
                    {
                        if (plant.def == ThingDefOf.Plant_TreeGauranlen || plant.def == ThingDefOf.Plant_TreeAnima) //Trees count 35x
                        {
                            plantTotalWeight += 35;
                            //specialTreeCount += 1;
                            continue;
                        }
                        plantTotalWeight += 1; //Trees count 1x
                        continue;
                    }
                    plantTotalWeight += 0.1f; //Other plants count 0.1x
                }
            }
            return (int)plantTotalWeight;
        }
    }
    public class CompProperties_NatureConnectionHediff : HediffCompProperties
    {
        public CompProperties_NatureConnectionHediff()
        {
            this.compClass = typeof(Comp_NatureConnectionHediff);
        }
     }
    
}
