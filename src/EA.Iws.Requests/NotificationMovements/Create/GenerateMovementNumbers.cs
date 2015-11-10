namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GenerateMovementNumbers : IRequest<IList<int>>
    {
        public int NewMovementsCount { get; private set; }
        public Guid NotificationId { get; private set; }

        public GenerateMovementNumbers(Guid notificationId, int newMovementsCount)
        {
            NotificationId = notificationId;
            NewMovementsCount = newMovementsCount;
        }
    }
}