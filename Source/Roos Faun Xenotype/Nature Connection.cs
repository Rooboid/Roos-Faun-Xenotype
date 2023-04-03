using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;


namespace Roos_Faun_Xenotype
{

    public class RBSF_NaturalConnection : Gene
    {
        // Grows plans in an area around naturally connected pawns
        public override void Tick()
        {
            base.Tick();

            //Return if the hash interval is incorrect (not enough time has passed)
            if (!this.pawn.IsHashIntervalTick(600))
            {
                return;
            }

            Log.Message("Natural Connection scanning");

            //Return if the pawn is not spawned
            if (!this.pawn.Spawned)
            {
                return;
            }

            if (this.pawn.Map == null) {
                return;
            }

            List<Thing> mapThings = this.pawn.Map.listerThings.AllThings;
            foreach (Thing thing in mapThings)
            {
                if (thing.def.category == ThingCategory.Plant && thing.Position.InHorDistOf(this.pawn.Position, 15)) {
                    Plant plant = thing as Plant;
                    if (plant != null)
                    {
                        if (plant.LifeStage != PlantLifeStage.Mature) {
                            Log.Message("Plant found at " + plant.Position + " Lifestage is " + plant.LifeStage + " tried to grow.");
                            plant.Growth += 0.05f;
                            FleckMaker.Static(plant.Position, plant.Map, RBSF_DefOf.DustPuff, 1f);
                            break;
                        }
                    }
                }
            }
        }
    }
}
