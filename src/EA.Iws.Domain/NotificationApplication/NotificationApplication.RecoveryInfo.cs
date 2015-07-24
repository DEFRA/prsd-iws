namespace EA.Iws.Domain.NotificationApplication
{
    using Core.RecoveryInfo;

    public partial class NotificationApplication
    {
        public bool HasRecoveryInfo
        {
            get { return RecoveryInfo != null; }
        }

        public RecoveryInfo SetRecoveryInfoValues(RecoveryInfoUnits estimatedUnit, decimal estimatedAmount, RecoveryInfoUnits costUnit, decimal costAmount,
            RecoveryInfoUnits? disposalUnit, decimal? disposalAmount)
        {
            RecoveryInfo = new RecoveryInfo(estimatedUnit, estimatedAmount, costUnit, costAmount,
                disposalUnit, disposalAmount);
            return RecoveryInfo;
        }
    }
}
