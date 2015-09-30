namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Prsd.Core.Mediator;

    public class GetRecoverablePercentage : IRequest<decimal?>
    {
        public Guid NotificationId { get; private set; }

        public GetRecoverablePercentage(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
