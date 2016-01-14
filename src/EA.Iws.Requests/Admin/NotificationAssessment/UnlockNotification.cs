namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class UnlockNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public UnlockNotification(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}