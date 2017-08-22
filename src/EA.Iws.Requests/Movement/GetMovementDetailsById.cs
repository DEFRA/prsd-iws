namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementDetailsById : IRequest<MovementBasicDetails>
    {
        public GetMovementDetailsById(Guid notificationId, Guid movementId)
        {
            NotificationId = notificationId;
            MovementId = movementId;
        }

        public Guid NotificationId { get; private set; }

        public Guid MovementId { get; private set; }
    }
}
