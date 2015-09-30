namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetWasteRecoveryProvider : IRequest<ProvidedBy?>
    {
        public Guid NotificationId { get; private set; }

        public GetWasteRecoveryProvider(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
