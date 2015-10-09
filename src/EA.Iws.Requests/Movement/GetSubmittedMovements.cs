namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetSubmittedMovements : IRequest<List<SubmittedMovement>>
    {
        public Guid NotificationId { get; private set; }

        public GetSubmittedMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
