namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationIsInterimSetEvent : IEvent
    {
        public NotificationIsInterimSetEvent(Guid notificationApplicationId, bool isInterim)
        {
            IsInterim = isInterim;
            NotificationApplicationId = notificationApplicationId;
        }

        public Guid NotificationApplicationId { get; private set; }

        public bool IsInterim { get; private set; }
    }
}