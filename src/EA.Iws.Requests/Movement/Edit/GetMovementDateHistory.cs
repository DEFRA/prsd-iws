namespace EA.Iws.Requests.Movement.Edit
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementDateHistory : IRequest<IList<DateHistory>>
    {
        public Guid MovementId { get; private set; }

        public GetMovementDateHistory(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}