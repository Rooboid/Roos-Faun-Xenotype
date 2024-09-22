using RimWorld;
using System;
using Verse;

namespace Roos_Faun_Xenotype
{

    public class RBSF_NaturalConnection : Gene
    {

        // Randomly grows plants in an area around naturally connected pawns
        public override void Tick()
        {
            base.Tick();

            if (!this.pawn.Spawned)
                return;

            if (this.pawn.Map == null)
                return;

            if (!pawn.health.hediffSet.HasHediff(RBSF_DefOf.RBSF_NatureConnectionOffset))
            {
                pawn.health.AddHediff(RBSF_DefOf.RBSF_NatureConnectionOffset);
            }

            growRandomNearbyPlants(pawn);

        }

        public static bool growRandomNearbyPlants(Pawn pawn)
        {
            // Natural Connection - Plant Growth
            var natureConnection = pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            var hashTick = (int)(1200 / natureConnection);

            if (!pawn.IsHashIntervalTick(hashTick))
            {
                return false;
            }
                
            var plantGrowRadius = 5.5f;
            var numTilesToScan = 5;

            //Log.Message("Natural Connection scanning");

            //tries a random location around the pawn 'numRetries' times - if it finds a plant, progress its growth by 1 day.
            for (var i = 0; i < numTilesToScan; i++)
            {
                //get random nearby cell, and check if it has a plant on it
                var randLocInRadius = pawn.Position + (Rand.InsideUnitCircleVec3 * plantGrowRadius).ToIntVec3();
                if (!randLocInRadius.InBounds(pawn.Map))
                {
                    continue;
                }
                var plant = randLocInRadius.GetPlant(pawn.Map);
                if (plant == null || plant.LifeStage != PlantLifeStage.Growing)
                {
                    continue;
                }

                //Grow plants equal to total of 0.1 day of growing.
                //growDays is misleading due to days being 50% darkness
                var growthPerDay = 1f / plant.def.plant.growDays;
                var growthPerTrigger = growthPerDay / 10;
                plant.Growth += growthPerTrigger;

                //make fleck and redraw map area
                FleckMaker.Static(plant.Position, plant.Map, RBSF_DefOf.RBSF_NatureGrowthFleck, 2f);
                pawn.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlagDefOf.Things);

                //Log.Message("Plant found at " + plant.Position + " in iteration " + i);
            }
            return true;
        }
    }
}
