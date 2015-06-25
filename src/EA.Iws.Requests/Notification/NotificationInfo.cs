namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Shared;

    public class NotificationInfo
    {
        public Guid NotificationId { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public string CompetentAuthorityName { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
