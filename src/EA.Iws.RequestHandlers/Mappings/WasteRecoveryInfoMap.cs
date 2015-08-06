namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Core.RecoveryInfo;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class WasteRecoveryInfoMap : IMap<NotificationApplication, WasteRecoveryInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap;

        public WasteRecoveryInfoMap(IMap<NotificationApplication, NotificationApplicationCompletionProgress> completionProgressMap)
        {
            this.completionProgressMap = completionProgressMap;
        }

        public WasteRecoveryInfo Map(NotificationApplication notification)
        {
            return new WasteRecoveryInfo
            {
                NotificationId = notification.Id,
                NotificationType = notification.NotificationType == NotificationType.Disposal
                        ? Core.Shared.NotificationType.Disposal
                        : Core.Shared.NotificationType.Recovery,
                Progress = completionProgressMap.Map(notification),
                RecoveryInfoData = GetRecoveryInfo(notification),
                RecoveryPercentageData = GetRecoveryPercentage(notification)
            };
        }

        private static RecoveryInfoData GetRecoveryInfo(NotificationApplication notification)
        {
            var recoveryInfoData = new RecoveryInfoData();
            if (notification.RecoveryInfo != null)
            {
                recoveryInfoData.EstimatedAmount = notification.RecoveryInfo.EstimatedAmount;
                recoveryInfoData.EstimatedUnit = notification.RecoveryInfo.EstimatedUnit;
                recoveryInfoData.CostAmount = notification.RecoveryInfo.CostAmount;
                recoveryInfoData.CostUnit = notification.RecoveryInfo.CostUnit;
                recoveryInfoData.DisposalAmount = notification.RecoveryInfo.DisposalAmount;
                recoveryInfoData.DisposalUnit = notification.RecoveryInfo.DisposalUnit;
            }
            return recoveryInfoData;
        }

        private static RecoveryPercentageData GetRecoveryPercentage(NotificationApplication notification)
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
