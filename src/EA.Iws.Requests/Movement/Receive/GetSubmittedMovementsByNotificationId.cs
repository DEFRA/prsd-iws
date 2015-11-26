namespace EA.Iws.Requests.Movement.Receive
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetSubmittedMovementsByNotificationId : IRequest<IList<MovementData>>
    {
        public Guid Id { get; private set; }

        public GetSubmittedMovementsByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
