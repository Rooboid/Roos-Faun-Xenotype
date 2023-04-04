﻿using RimWorld;
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

            var natureConnectionSquared = (float)Math.Pow(this.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection), 2);
            var hashTick = (int) (1200 / natureConnectionSquared);

            if (!this.pawn.IsHashIntervalTick(hashTick))
                return;

            var plantGrowRadius = 5.5f;
            var numRetries = 20;

            Log.Message("Natural Connection scanning");

            //tries a random location around the pawn 'numRetries' times - if it finds a plant, progress its growth by 1 day.
            for (var i = 0; i < numRetries; i++) {

                var randLocInRadius = this.pawn.Position + (Rand.InsideUnitCircleVec3 * plantGrowRadius).ToIntVec3();
                var cellThings = randLocInRadius.GetThingList(this.pawn.Map);

                //check for plants on cell
                foreach (var thing in cellThings)
                {
                    if (thing.def.category != ThingCategory.Plant)
                    {
                        continue;
                    }

                    var plant = thing as Plant;
                    if (plant == null)
                    {
                        continue;
                    }

                    if (plant.LifeStage != PlantLifeStage.Growing)
                    {
                        break;
                    }

                    //Grow plant equal to 1 day of growing.
                    var growthPerDay = 1f / plant.def.plant.growDays;
                    var growthPerTrigger = growthPerDay / 2;
                    plant.Growth += growthPerTrigger;

                    //make fleck and redraw map area
                    FleckMaker.Static(plant.Position, plant.Map, RBSF_DefOf.RBSF_NatureGrowthFleck, 2f);
                    this.pawn.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlag.Things);

                    Log.Message("Plant found at " + plant.Position + " in iteration " + i);
                    return;
                    
                }
            }
        }
    }
}
