namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class SetRecoveryInfoProvider : IRequest<bool>
    {
        public ProvidedBy ProvidedBy { get; private set; }
        public Guid NotificationId { get; private set; }

        public SetRecoveryInfoProvider(ProvidedBy providedBy, Guid notificationId)
        {
            ProvidedBy = providedBy;
            NotificationId = notificationId;
        }
    }
}
