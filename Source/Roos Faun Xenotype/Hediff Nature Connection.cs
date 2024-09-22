using RimWorld;
using System;
using Verse;

namespace Roos_Faun_Xenotype
{
    public class RBSF_NatureConnectionOffsetComp : HediffComp
    {
        //Scales severity with number of nearby plants
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (base.Pawn.IsHashIntervalTick(60))
            {
                var numPlants = CountNearPlants(parent.pawn);
                Log.Message("counted " + numPlants + " plants near " + parent.pawn.Name.ToStringShort);
                var severity = 0;
                switch (numPlants)
                {
                    case int i when i < 10:
                        severity = 1;
                        break;
                    case int i when i < 20:
                        severity = 2;
                        break;
                    case int i when i < 30:
                        severity = 3;
                        break;
                    case int i when i < 40:
                        severity = 4;
                        break;
                    default:
                        severity = 5;
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
            int num = GenRadial.NumCellsInRadius(plantRadius);
            int plantCount = 0;
            for (int i = 0; i < num; i++)
            {
                //Scan radius around pawn for plants
                IntVec3 intVec = positionHeld + GenRadial.RadialPattern[i];
                Plant plant = intVec.GetPlant(mapHeld);

                //count plants
                if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && plant != null)
                {
                    if (plant.def.plant.IsTree && !plant.def.plant.isStump)
                    {
                        if (plant.def == ThingDefOf.Plant_TreeGauranlen || plant.def == ThingDefOf.Plant_TreeAnima) //Trees count 50x
                        {
                            plantCount += 50;
                        }
                        plantCount += 5; //Trees count 5x
                        continue;
                    }
                    plantCount += 1;
                }
            }
            return plantCount;
        }
    }
}
