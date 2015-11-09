namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetBasicMovementSummary : IRequest<BasicMovementSummary>
    {
        public Guid NotificationId { get; private set; }

        public GetBasicMovementSummary(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}