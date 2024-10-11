using Verse;

namespace Roos_Faun_Xenotype
{

    public class RBSF_NaturalConnection : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            Hediff natureConnectionHediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(RBSF_DefOf.RBSF_NatureConnectionOffset, false);
            if (natureConnectionHediff == null)
            {
                this.pawn.health.AddHediff(RBSF_DefOf.RBSF_NatureConnectionOffset);
            }
        }

        public override void PostRemove()
        {
            Hediff natureConnectionHediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(RBSF_DefOf.RBSF_NatureConnectionOffset, false);
            if (natureConnectionHediff != null)
            {
                this.pawn.health.RemoveHediff(natureConnectionHediff);
            }
            base.PostRemove();
        }
    }
}
