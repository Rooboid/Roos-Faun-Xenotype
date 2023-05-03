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
            //target.Pawn?.needs?.mood?.thoughts?.memories?.TryGainMemory(RBSF_DefOf.RBSFE_ConstrictedThought);

            base.Apply(target, dest);
            
            target.Pawn.stances.stunner.StunFor(Props.stunDuration, this.parent.pawn);
            //MoteMaker.MakeThoughtBubble(this.parent.pawn, "Textures/Things/Mote/ChokevineThought/RBSF_ChokevineThought.png", true);
            //Thought thought = ThoughtMaker.MakeThought(RBSF_DefOf.RBSFE_ConstrictedThought);
            


        }

        //  Adjusts range to scale with nature connection on gizmo update.
        public override void OnGizmoUpdate()
        {
            base.OnGizmoUpdate();

            var adjustedRange = 5 * this.parent.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            Log.Message("Range adjusted to " + adjustedRange.ToString() + " by gizmo update");

            this.parent.pawn.abilities.GetAbility(RBSF_DefOf.RBSF_AbilityChokevineGrasp).verb.verbProps.range = adjustedRange;
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

        //  Adjusts range to scale with nature connection when casting.
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            var adjustedRange = 5 * this.caster.GetStatValue(RBSF_DefOf.RBSF_NatureConnection);
            Log.Message("Range adjusted to " + adjustedRange.ToString());
            this.verbProps.range = adjustedRange;

            return base.ValidateTarget(target, showMessages);

        }
    }

    
}
