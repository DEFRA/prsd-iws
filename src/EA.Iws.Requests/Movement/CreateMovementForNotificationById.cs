namespace EA.Iws.Requests.Movement
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateMovementForNotificationById : IRequest<Guid>
    {
        public Guid Id { get; private set; }

        public CreateMovementForNotificationById(Guid id)
        {
            Id = id;
        }
    }
}
