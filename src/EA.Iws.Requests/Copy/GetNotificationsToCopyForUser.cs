namespace EA.Iws.Requests.Copy
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationsToCopyForUser : IRequest<IList<NotificationApplicationCopyData>>
    {
        public GetNotificationsToCopyForUser(Guid destinationNotificationId)
        {
            DestinationNotificationId = destinationNotificationId;
        }

        public Guid DestinationNotificationId { get; private set; }
    }
}