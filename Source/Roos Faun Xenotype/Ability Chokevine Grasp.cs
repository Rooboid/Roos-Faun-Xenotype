using RimWorld;
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
            var adjustedRange = 5 + (8 * parent.pawn.GetStatValue(RBSF_DefOf.RBSF_NatureConnection));
            this.parent.pawn.abilities.GetAbility(RBSF_DefOf.RBSF_AbilityChokevineGrasp).verb.verbProps.range = adjustedRange;
            base.OnGizmoUpdate();
        }
    }

    public class RBSF_CompProperties_AbilityChokevineGrasp : CompProperties_AbilityEffect
    {
        public RBSF_CompProperties_AbilityChokevineGrasp()
        {
            this.compClass = typeof(RBSF_CompAbilityEffect_ChokevineGrasp);
        }
        public int stunDuration = 720;
    }

    public class RBSF_Verb_CastAbilityChokevineGrasp : Verb_CastAbility
    {

        //  Adjusts range to scale with nature connection when casting.
        public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            var adjustedRange = 5 + (8 * caster.GetStatValue(RBSF_DefOf.RBSF_NatureConnection));
            this.verbProps.range = adjustedRange;
            return base.ValidateTarget(target, showMessages);

        }
    }
}
