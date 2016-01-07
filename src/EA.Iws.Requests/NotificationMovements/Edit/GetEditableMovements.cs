namespace EA.Iws.Requests.NotificationMovements.Edit
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetEditableMovements : IRequest<IEnumerable<MovementData>>
    {
        public Guid NotificationId { get; private set; }

        public GetEditableMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}