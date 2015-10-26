namespace EA.Iws.Requests.Movement.Summary
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementsSummaryByNotificationId : IRequest<MovementSummaryData>
    {
        public Guid Id { get; private set; }

        public MovementStatus? Status { get; private set; }

        public GetMovementsSummaryByNotificationId(Guid id, MovementStatus? status)
        {
            Id = id;
            Status = status;
        }
    }
}
