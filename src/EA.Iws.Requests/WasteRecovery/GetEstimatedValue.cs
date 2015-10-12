namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetEstimatedValue : IRequest<ValuePerWeightData>
    {
        public Guid NotificationId { get; private set; }

        public GetEstimatedValue(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
