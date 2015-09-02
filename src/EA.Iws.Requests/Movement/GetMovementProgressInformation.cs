namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementProgressInformation : IRequest<MovementProgressAndSummaryData>
    {
        public Guid NotificationId { get; private set; }

        public Guid MovementId { get; private set; }

        public GetMovementProgressInformation(Guid notificationId, Guid movementId)
        {
            NotificationId = notificationId;
            MovementId = movementId;
        }
    }
}
