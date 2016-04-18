namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetBasicMovementSummary : IRequest<BasicMovementSummary>
    {
        public Guid NotificationId { get; private set; }

        public GetBasicMovementSummary(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}