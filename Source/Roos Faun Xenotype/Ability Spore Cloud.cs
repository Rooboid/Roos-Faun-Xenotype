using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Roos_Faun_Xenotype
{


    public class CompAbilityEffect_SporeCloud : CompAbilityEffect
    {

        //Main 'Spore Cloud' ability method - gives buffs to allies and debuffs to enemies
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = this.parent.pawn;
            IntVec3 position = target.Cell;
            Map map = pawn.Map;
            float radius = 2.5f * this.parent.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);

            base.Apply(target, dest);

            GenExplosion.DoExplosion(target.Cell, map, radius, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
            SporeExplodeInArea(position, map, radius, pawn);
        }

        //Draws radius circle
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            var adjustedRadius = 2.5f * this.parent.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            GenDraw.DrawRadiusRing(target.Cell, adjustedRadius);
        }

        //Allows AI to use ability only if 35% to pain threshold
        public override bool AICanTargetNow(LocalTargetInfo target)
        {
            float? currentPain = this.parent?.pawn?.health?.hediffSet?.PainTotal;
            float allowedPain = this.parent.pawn.GetStatValue(StatDefOf.PainShockThreshold) * 0.3f;

            if (currentPain > allowedPain)
            {
                return true;
            }
            return false;
        }
        public static bool SporeExplodeInArea(IntVec3 position, Map map, float radius, Pawn caster)
        {
            if (map == null)
            {
                return false;
            }

            List<Pawn> mapPawns = (List<Pawn>)map.mapPawns.AllPawnsSpawned;

            for (int i = 0; i < mapPawns.Count; i++)
            {
                Pawn pawn = mapPawns[i];
                bool isInRange = position.InHorDistOf(mapPawns[i].Position, radius);
                bool notMech = pawn.RaceProps?.IsMechanoid == false;

                if (isInRange && notMech) //Tox mask check?
                {
                    if (pawn.HostileTo(caster))
                    {
                        pawn.health.AddHediff(RBSF_DefOf.RBSF_BadTripHediff);
                        continue;
                    }
                    //Good stuff
                    pawn.health.AddHediff(RBSF_DefOf.RBSF_GoodTripHediff);
                }
            }
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = position + GenRadial.RadialPattern[i];
                RBSF_DefOf.RBSF_SporeCloudExplosionEffect.Spawn(intVec, map, 1f);
            }
            return false;
        }
    }


    public class CompProperties_AbilitySporeCloud : CompProperties_AbilityEffect
    {
        public CompProperties_AbilitySporeCloud()
        {
            this.compClass = typeof(CompAbilityEffect_SporeCloud);
        }
    }

    public class RBSF_Verb_CastAbilitySporeCloud : Verb_CastAbility
    {

        //  Adjusts range to scale with nature connection when casting.
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            var adjustedRange = 4 + 4 * this.caster.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            //Log.Message("Range adjusted to " + adjustedRange.ToString());
            this.verbProps.range = adjustedRange;
            return base.ValidateTarget(target, showMessages);

        }
    }
}
