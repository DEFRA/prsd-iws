namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class SetNotificationLocalAreaId : IRequest<Guid>
    {
        public SetNotificationLocalAreaId(Guid notificationId, Guid localAreaId)
        {
            LocalAreaId = localAreaId;
            NotificationId = notificationId;
        }

        public Guid LocalAreaId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
