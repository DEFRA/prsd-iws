namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementSummary : IRequest<MovementSummary>
    {
        public Guid Id { get; private set; }

        public GetMovementSummary(Guid id)
        {
            Id = id;
        }
    }
}
