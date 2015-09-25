namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.RecoveryInfo;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class RecoveryInfoMap : IMap<RecoveryInfo, RecoveryInfoData>
    {
        public RecoveryInfoData Map(RecoveryInfo source)
        {
            RecoveryInfoData data = null;

            if (source != null)
            {
                data = new RecoveryInfoData
                {
                    NotificationId = source.NotificationId,
                    EstimatedUnit = source.EstimatedValue.Units,
                    EstimatedAmount = source.EstimatedValue.Amount,
                    CostUnit = source.RecoveryCost.Units,
                    CostAmount = source.RecoveryCost.Amount
                };

                if (source.DisposalCost != null)
                {
                    data.DisposalUnit = source.DisposalCost.Units;
                    data.DisposalAmount = source.DisposalCost.Amount;
                }
            }

            return data;
        }
    }
}