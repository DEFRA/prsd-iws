namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.Shared;
    using Requests.Notification.Overview;

    public class WasteRecoveryViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsWasteRecoveryInformationCompleted { get; set; }

        public WasteRecoveryViewModel()
        {
        }

        public WasteRecoveryViewModel(WasteRecovery wasteRecoveryInfo)
        {
            NotificationId = wasteRecoveryInfo.NotificationId;
//          IsWasteRecoveryInformationCompleted = wasteRecoveryInfo.IsWasteRecoveryInformationCompleted;
        }
    }
}