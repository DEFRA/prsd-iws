namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class GetNotificationLocalAreaId : IRequest<Guid>
    {
        public GetNotificationLocalAreaId(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}
