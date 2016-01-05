namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementRulesSummary : IRequest<MovementRulesSummary>
    {
        public GetMovementRulesSummary(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}