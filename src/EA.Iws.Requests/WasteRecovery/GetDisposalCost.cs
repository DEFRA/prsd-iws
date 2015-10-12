namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class GetDisposalCost : IRequest<ValuePerWeightData>
    {
        public Guid NotificationId { get; private set; }

        public GetDisposalCost(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
