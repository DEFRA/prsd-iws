namespace EA.Iws.Requests.Admin.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class AcceptChanges : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public AcceptChanges(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}