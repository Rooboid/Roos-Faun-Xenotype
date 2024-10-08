﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Roos_Faun_Xenotype
{
    public class Comp_NatureConnectionHediff : HediffComp
    {
        //Scales severity with number of nearby plants
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (base.Pawn.IsHashIntervalTick(120))
            {
                var numPlants = CountNearPlants(parent.pawn);
                //Log.Message(numPlants + " plant score near " + Pawn.Name.ToStringShort);
                //Log.Message("counted " + numPlants + " plants near " + parent.pawn.Name.ToStringShort);
                float severity;
                switch (numPlants)
                {
                    case int i when i < 5:
                        severity = 0.5f;
                        break;
                    case int i when i < 10:
                        severity = 1;
                        break;
                    case int i when i < 15:
                        severity = 2;
                        break;
                    case int i when i < 25:
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

            float plantSearchRadius = 5f;

            // List all plants
            IEnumerable<IntVec3> cellList = GenRadial.RadialCellsAround(pawn.PositionHeld, plantSearchRadius, true);

            var nearPlantsList = new List<Thing>();
            foreach (var cell in cellList)
            {
                if (cell == null || !cell.InBounds(mapHeld) || cell.Fogged(mapHeld)) continue;
                Plant plant = cell.GetPlant(mapHeld);
                if (plant == null) continue;
                nearPlantsList.Add(plant);
            }

            float plantTotalWeight = 0;
            foreach (Thing plant in nearPlantsList)
            {
                if (!plant.def.plant.IsTree) // Non-tree plants
                {
                    plantTotalWeight += 0.3f;
                    continue;
                }
                if (plant.def == ThingDefOf.Plant_TreeGauranlen || plant.def == ThingDefOf.Plant_TreeAnima) // Special Trees
                {
                    plantTotalWeight += 15;
                    continue;
                }
                if (!plant.def.plant.isStump) // Regular Trees
                {
                    plantTotalWeight += 1;
                    continue;
                }
                plantTotalWeight -= 0.5f; // Tree Stump
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
