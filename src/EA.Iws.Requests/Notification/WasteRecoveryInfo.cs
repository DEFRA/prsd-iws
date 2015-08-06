namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.RecoveryInfo;
    using Core.Shared;

    public class WasteRecoveryInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public RecoveryPercentageData RecoveryPercentageData { get; set; }
        public RecoveryInfoData RecoveryInfoData { get; set; }
    }
}
