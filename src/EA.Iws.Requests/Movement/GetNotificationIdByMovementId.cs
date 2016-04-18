namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetNotificationIdByMovementId : IRequest<Guid>
    {
        public Guid MovementId { get; private set; }

        public GetNotificationIdByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
