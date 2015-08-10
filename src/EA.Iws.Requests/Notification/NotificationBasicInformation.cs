namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Core.Shared;

    public class NotificationBasicInfo
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }
    }
}