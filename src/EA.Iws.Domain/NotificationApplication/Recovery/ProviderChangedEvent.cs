namespace EA.Iws.Domain.NotificationApplication.Recovery
{
    using System;
    using Core.Shared;
    using Prsd.Core.Domain;

    public class ProviderChangedEvent : IEvent
    {
        public Guid NotificationId { get; set; }
        public ProvidedBy NewProvider { get; private set; }

        public ProviderChangedEvent(Guid notificationId, ProvidedBy newProvider)
        {
            NotificationId = notificationId;
            NewProvider = newProvider;
        }
    }
}
