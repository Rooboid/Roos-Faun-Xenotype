using RimWorld;
using RimWorld.Planet;
using Verse;

namespace Roos_Faun_Xenotype
{
    public class CompAbilityEffect_Treewalk : CompAbilityEffect
    {
        //Main 'Treewalk' ability method - teleports a pawn to a tree
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            Pawn pawn = this.parent.pawn;
            IntVec3 position = target.Cell;
            Map map = pawn.Map;
            base.Apply(target, dest);
            SkipUtility.SkipTo(pawn, position, map);
        }

        //Draws radius circle
        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            var radius = 0.9f;
            GenDraw.DrawRadiusRing(target.Cell, radius);
        }

        public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
        {
            if (target.Cell.GetPlant(target.Map).def.plant.IsTree && !target.Cell.GetPlant(target.Map).def.plant.isStump)
            {
                return true;
            }
            return false;
        }

        public override bool GizmoDisabled(out string reason)
        {
            //if (MinotaurSettings.debugMessages) { Log.Message("RBM Is Running: (AbilityLactate) public override bool GizmoDisabled(out string reason)"); }

            if (parent.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection) < 1.8)
            {
                reason = "Connection to nature too weak.";
                return true;
            }

            if (base.GizmoDisabled(out reason))
            {
                return true;
            }
            return false;
        }
    }
    public class CompProperties_AbilityTreewalk : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityTreewalk()
        {
            this.compClass = typeof(CompAbilityEffect_Treewalk);
        }
    }

    public class RBSF_Verb_CastAbilityTreewalk : Verb_CastAbility
    {
        //  Adjusts range to scale with nature connection when casting.
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {

            // adjust range
            var adjustedRange = 16 * this.caster.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            this.verbProps.range = adjustedRange;

            if (base.ValidateTarget(target, showMessages) == false)
            {
                return false;
            }

            // validate target is tree
            if (target.Cell != null && target.Cell.InBounds(this.caster.Map) && !target.Cell.Fogged(this.caster.Map))
            {
                PlantProperties plantProps = target.Cell.GetPlant(this.caster.Map)?.def?.plant;
                if (plantProps != null && plantProps.IsTree && !plantProps.isStump)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
