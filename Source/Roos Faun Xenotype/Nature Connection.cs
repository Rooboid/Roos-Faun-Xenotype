using RimWorld;
using System.Collections.Generic;
using System;
using Verse;
using RimWorld.QuestGen;

namespace Roos_Faun_Xenotype
{

    public class RBSF_NaturalConnection : Gene
    {
        // Randomly grows plants in an area around naturally connected pawns
        public override void Tick()
        {
            base.Tick();

            if (!this.pawn.Spawned)
            {
                return;
            }

            if (this.pawn.Map == null)
            {
                return;
            }

            float NatureConnectionSquared = (float)Math.Pow(this.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection), 2);
            int hashTick = (int) (1200 / NatureConnectionSquared);
            if (!this.pawn.IsHashIntervalTick(hashTick))
            {
                return;
            }

            float plantGrowRadius = 5.5f;
            int numRetries = 20;

            Log.Message("Natural Connection scanning");

            //tries a random location around the pawn 'numRetries' times - if it finds a plant, progress its growth by 1 day.
            for (int i = 0; i < numRetries; i++) {

                IntVec3 randLocInRadius = this.pawn.Position + (Rand.InsideUnitCircleVec3 * plantGrowRadius).ToIntVec3();
                List<Thing> cellThings = randLocInRadius.GetThingList(this.pawn.Map);

                //check for plants on cell
                foreach (Thing thing in cellThings)
                {
                    if (thing.def.category != ThingCategory.Plant)
                    {
                        continue;
                    }

                    Plant plant = thing as Plant;
                    if (plant == null)
                    {
                        continue;
                    }

                    if (plant.LifeStage != PlantLifeStage.Growing)
                    {
                        break;
                    }

                    //Grow plant equal to 1 day of growing.
                    float GrowthPerDay = 1f / plant.def.plant.growDays;
                    float GrowthPerTrigger = GrowthPerDay / 2;
                    plant.Growth += GrowthPerTrigger;

                    //make fleck and redraw map area
                    FleckMaker.Static(plant.Position, plant.Map, RBSF_DefOf.NatureGrowthFleck, 2f);
                    this.pawn.Map.mapDrawer.MapMeshDirty(plant.Position, MapMeshFlag.Things);

                    Log.Message("Plant found at " + plant.Position + " in interation " + i);
                    return;
                    
                }
            }
        }
    }
}
