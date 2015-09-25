namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Linq;
    using Core.Notification;
    using Core.RecoveryInfo;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class WasteRecoveryInfoMap : IMap<NotificationApplication, WasteRecoveryInfo>
    {
        private readonly IwsContext context;

        public WasteRecoveryInfoMap(IwsContext context)
        {
            this.context = context;
        }

        public WasteRecoveryInfo Map(NotificationApplication notification)
        {
            return new WasteRecoveryInfo
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                RecoveryInfoData = GetRecoveryInfo(notification),
                RecoveryPercentageData = GetRecoveryPercentage(notification)
            };
        }

        private RecoveryInfoData GetRecoveryInfo(NotificationApplication notification)
        {
            var recoveryInfoData = new RecoveryInfoData();

            var recoveryInfo = context.RecoveryInfos.SingleOrDefault(ri => ri.NotificationId == notification.Id);

            if (recoveryInfo != null)
            {
                recoveryInfoData.EstimatedAmount = recoveryInfo.EstimatedValue.Amount;
                recoveryInfoData.EstimatedUnit = recoveryInfo.EstimatedValue.Units;
                recoveryInfoData.CostAmount = recoveryInfo.RecoveryCost.Amount;
                recoveryInfoData.CostUnit = recoveryInfo.RecoveryCost.Units;
               
                if (recoveryInfo.DisposalCost != null)
                {
                    recoveryInfoData.DisposalAmount = recoveryInfo.DisposalCost.Amount;
                    recoveryInfoData.DisposalUnit = recoveryInfo.DisposalCost.Units;
                }
            }

            return recoveryInfoData;
        }

        private RecoveryPercentageData GetRecoveryPercentage(NotificationApplication notification)
        {
            var recoveryPercentageData = new RecoveryPercentageData
            {
                IsProvidedByImporter = notification.IsProvidedByImporter,
                PercentageRecoverable = notification.PercentageRecoverable,
                MethodOfDisposal = notification.MethodOfDisposal ?? string.Empty
            };
            return recoveryPercentageData;
        }
    }
}
