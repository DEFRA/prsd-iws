namespace EA.Iws.Domain.NotificationApplication.WasteRecovery
{
    using System;
    using Prsd.Core.Domain;

    public class PercentageChangedEvent : IEvent
    {
        public Guid NotificationId { get; private set; }
        public Percentage NewPercentage { get; private set; }

        public PercentageChangedEvent(Guid notificationId, Percentage newPercentage)
        {
            NotificationId = notificationId;
            NewPercentage = newPercentage;
        }
    }
}
