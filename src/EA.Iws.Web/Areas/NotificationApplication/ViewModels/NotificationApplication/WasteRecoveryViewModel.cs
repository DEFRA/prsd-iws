namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Notification;
    using Core.RecoveryInfo;
    using Core.Shared;
    using Requests.Notification;

    public class WasteRecoveryViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsWasteRecoveryInformationCompleted { get; set; }
        public RecoveryPercentageData RecoveryPercentageData { get; set; }
        public RecoveryInfoData RecoveryInfoData { get; set; }

        public WasteRecoveryViewModel()
        {
        }

        public WasteRecoveryViewModel(WasteRecoveryInfo wasteRecoveryInfo)
        {
            NotificationId = wasteRecoveryInfo.NotificationId;
            NotificationType = wasteRecoveryInfo.NotificationType;
            IsWasteRecoveryInformationCompleted = wasteRecoveryInfo.IsWasteRecoveryInformationCompleted;
            RecoveryPercentageData = wasteRecoveryInfo.RecoveryPercentageData;
            RecoveryInfoData = wasteRecoveryInfo.RecoveryInfoData;
        }
    }
}