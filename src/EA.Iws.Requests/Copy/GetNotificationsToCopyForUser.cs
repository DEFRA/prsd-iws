namespace EA.Iws.Requests.Copy
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GetNotificationsToCopyForUser : IRequest<IList<NotificationApplicationCopyData>>
    {
        public GetNotificationsToCopyForUser(Guid destinationNotificationId)
        {
            DestinationNotificationId = destinationNotificationId;
        }

        public Guid DestinationNotificationId { get; private set; }
    }
}