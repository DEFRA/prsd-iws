namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.RecoveryInfo;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.RecoveryInfo;

    internal class RecoveryInfoMap : IMap<NotificationApplication, RecoveryInfoData>
    {
        public RecoveryInfoData Map(NotificationApplication source)
        {
            RecoveryInfoData data = null;
            if (source.HasRecoveryInfo)
            {
                data = new RecoveryInfoData
                {
                    NotificationId = source.Id,
                    EstimatedUnit = source.RecoveryInfo.EstimatedUnit,
                    EstimatedAmount = source.RecoveryInfo.EstimatedAmount,
                    CostUnit = source.RecoveryInfo.CostUnit,
                    CostAmount = source.RecoveryInfo.CostAmount,
                    DisposalUnit = source.RecoveryInfo.DisposalUnit,
                    DisposalAmount = source.RecoveryInfo.DisposalAmount
                };
            }
            return data;
        }
    }
}