namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using Prsd.Core.Domain;

    public class NotificationSubmittedEvent : IEvent
    {
        public Guid NotificationApplicationId { get; private set; }

        public NotificationSubmittedEvent(Guid notificationApplicationId)
        {
            NotificationApplicationId = notificationApplicationId;
        }
    }
}