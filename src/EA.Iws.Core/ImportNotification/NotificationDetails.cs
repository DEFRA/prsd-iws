namespace EA.Iws.Core.ImportNotification
{
    using System;
    using ImportNotificationAssessment;
    using Notification;
    using Shared;

    public class NotificationDetails
    {
        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public string NotificationNumber { get; set; }

        public ImportNotificationStatus Status { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public string Area { get; set; }
    }
}