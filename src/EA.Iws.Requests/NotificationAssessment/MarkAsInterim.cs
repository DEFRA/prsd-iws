namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class MarkAsInterim : IRequest<bool>
    {
        public MarkAsInterim(Guid notificationId, bool isInterim)
        {
            NotificationId = notificationId;
            IsInterim = isInterim;
        }

        public Guid NotificationId { get; private set; }

        public bool IsInterim { get; private set; }
    }
}