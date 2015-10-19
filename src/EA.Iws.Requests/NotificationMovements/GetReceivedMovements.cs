namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.MovementOperation;
    using Prsd.Core.Mediator;

    public class GetReceivedMovements : IRequest<MovementOperationData>
    {
        public Guid NotificationId { get; private set; }

        public GetReceivedMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
