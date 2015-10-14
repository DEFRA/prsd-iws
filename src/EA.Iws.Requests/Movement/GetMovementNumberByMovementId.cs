namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class GetMovementNumberByMovementId : IRequest<int>
    {
        public Guid MovementId { get; set; }

        public GetMovementNumberByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
