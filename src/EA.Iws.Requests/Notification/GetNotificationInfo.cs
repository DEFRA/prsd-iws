namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationInfo : IRequest<CompetentAuthorityData>
    {
        public Guid NotificationId { get; set; }

        public GetNotificationInfo(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
