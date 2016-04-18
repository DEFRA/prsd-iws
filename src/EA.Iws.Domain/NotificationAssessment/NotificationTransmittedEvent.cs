namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationTransmittedEvent : IEvent
    {
        public Guid NotificationApplicationId { get; private set; }

        public NotificationTransmittedEvent(Guid notificationApplicationId)
        {
            NotificationApplicationId = notificationApplicationId;
        }
    }
}
