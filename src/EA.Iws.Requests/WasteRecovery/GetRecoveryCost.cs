namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Prsd.Core.Mediator;

    public class GetRecoveryCost : IRequest<ValuePerWeightData>
    {
        public Guid NotificationId { get; private set; }

        public GetRecoveryCost(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
