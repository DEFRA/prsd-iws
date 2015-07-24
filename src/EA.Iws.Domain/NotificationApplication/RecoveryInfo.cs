namespace EA.Iws.Domain.Notification
{
    using Core.RecoveryInfo;
    using Prsd.Core.Domain;

    public class RecoveryInfo : Entity
    {
        public RecoveryInfoUnits EstimatedUnit { get; internal set; }
        public RecoveryInfoUnits CostUnit { get; internal set; }
        public RecoveryInfoUnits? DisposalUnit { get; internal set; }

        public decimal EstimatedAmount { get; internal set; }
        public decimal CostAmount { get; internal set; }
        public decimal? DisposalAmount { get; internal set; }

        protected RecoveryInfo()
        {
        }

        internal RecoveryInfo(RecoveryInfoUnits estimatedUnit, decimal estimatedAmount, RecoveryInfoUnits costUnit, decimal costAmount, 
            RecoveryInfoUnits? disposalUnit, decimal? disposalAmount)
        {
            EstimatedUnit = estimatedUnit;
            CostUnit = costUnit;
            DisposalUnit = disposalUnit;
            EstimatedAmount = estimatedAmount;
            CostAmount = costAmount;
            DisposalAmount = disposalAmount;
        }
    }
}
