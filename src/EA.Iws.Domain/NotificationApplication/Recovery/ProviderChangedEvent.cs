namespace EA.Iws.Domain.NotificationApplication.Recovery
{
    using System;
    using Core.Shared;
    using Prsd.Core.Domain;

    public class ProviderChangedEvent : IEvent
    {
        public Guid NotificationId { get; set; }
        public ProvidedBy ProvidedBy { get; private set; }

        public ProviderChangedEvent(Guid notificationId, ProvidedBy providedBy)
        {
            NotificationId = notificationId;
            ProvidedBy = providedBy;
        }
    }
}
