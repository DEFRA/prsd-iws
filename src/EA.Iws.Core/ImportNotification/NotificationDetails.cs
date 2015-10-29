namespace EA.Iws.Core.ImportNotification
{
    using System;
    using Shared;

    public class NotificationDetails
    {
        public Guid ImportNotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public string NotificatioNumber { get; set; }
    }
}