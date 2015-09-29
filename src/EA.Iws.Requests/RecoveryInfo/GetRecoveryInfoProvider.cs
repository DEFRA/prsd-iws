namespace EA.Iws.Requests.RecoveryInfo
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetRecoveryInfoProvider : IRequest<ProvidedBy?>
    {
        public Guid NotificationId { get; private set; }

        public GetRecoveryInfoProvider(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
