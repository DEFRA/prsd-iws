namespace EA.Iws.Requests.NotificationMovements.Capture
{
    using System;
    using Prsd.Core.Mediator;
    public class GetNextAvailableMovementNumberForNotification : IRequest<int>
    {
        public Guid Id { get; private set; }

        public GetNextAvailableMovementNumberForNotification(Guid id)
        {
            Id = id;
        }
    }
}
