namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using EA.Prsd.Core.Mediator;

    public class GetMovementsForNotificationById : IRequest<Dictionary<int, Guid>>
    {
        public Guid NotificationId { get; private set; }

        public GetMovementsForNotificationById(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
