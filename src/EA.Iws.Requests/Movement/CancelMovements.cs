namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class CancelMovements : IRequest<bool>
    {
        public IEnumerable<MovementData> CancelledMovements { get; private set; }
        public Guid NotificationId { get; set; }

        public CancelMovements(Guid notificationId, IEnumerable<MovementData> cancelledMovements)
        {
            NotificationId = notificationId;
            CancelledMovements = cancelledMovements;
        }
    }
}
