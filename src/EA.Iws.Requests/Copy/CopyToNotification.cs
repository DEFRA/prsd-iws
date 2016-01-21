namespace EA.Iws.Requests.Copy
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanCreateExportNotification)]
    public class CopyToNotification : IRequest<Guid>
    {
        public Guid SourceId { get; private set; }

        public Guid DestinationId { get; private set; }

        public CopyToNotification(Guid sourceId, Guid destinationId)
        {
            SourceId = sourceId;
            DestinationId = destinationId;
        }
    }
}
