using RimWorld;
using Roos_Faun_Xenotype;
using Verse;

namespace Roos_Faun_Xenotype
{
    public class RBSF_CompAbilityEffect_ChokevineGrasp : CompAbilityEffect
    {
        public new RBSF_CompProperties_AbilityChokevineGrasp Props
        {
            get
            {
                return (RBSF_CompProperties_AbilityChokevineGrasp)this.props;
            }
        }

        //Main 'Chokevine Grasp' ability method - ensnares pawns.
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            
            target.Pawn.stances.stunner.StunFor(Props.stunDuration, this.parent.pawn);
        }
    }

    public class RBSF_CompProperties_AbilityChokevineGrasp : CompProperties_AbilityEffect
    {
        public RBSF_CompProperties_AbilityChokevineGrasp()
        {
            this.compClass = typeof(RBSF_CompAbilityEffect_ChokevineGrasp);
        }
        public int stunDuration = 360;
    }

    public class RBSF_Verb_CastAbilityChokevineGrasp : Verb_CastAbilityTouch
    {
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            var adjustedRange = 5 * this.caster.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            Log.Message("Range adjusted to " + adjustedRange.ToString());
            this.verbProps.range = adjustedRange;

            return base.ValidateTarget(target, showMessages);

        }
    }
}
