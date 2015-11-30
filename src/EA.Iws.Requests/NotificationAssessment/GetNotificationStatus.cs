namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    public class GetNotificationStatus : IRequest<NotificationStatus>
    {
        public Guid NotificationId { get; private set; }

        public GetNotificationStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}