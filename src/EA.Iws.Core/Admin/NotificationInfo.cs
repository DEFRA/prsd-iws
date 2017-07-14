namespace EA.Iws.Core.Admin
{
    using System;
    using ImportNotificationAssessment;
    using NotificationAssessment;
    using Shared;

    public class NotificationInfo
    {
        public bool IsExistingNotification { get; set; }

        public Guid? Id { get; set; }

        public NotificationStatus? ExportNotificationStatus { get; set; }

        public ImportNotificationStatus? ImportNotificationStatus { get; set; }

        public TradeDirection? TradeDirection { get; set; }
    }
}