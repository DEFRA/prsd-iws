namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class GetInternalMovementSummary : IRequest<InternalMovementSummary>
    {
        public Guid NotificationId { get; private set; }

        public GetInternalMovementSummary(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
