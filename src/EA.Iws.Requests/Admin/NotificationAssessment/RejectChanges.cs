namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class RejectChanges : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public RejectChanges(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}