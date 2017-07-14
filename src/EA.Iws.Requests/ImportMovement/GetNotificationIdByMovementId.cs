namespace EA.Iws.Requests.ImportMovement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetNotificationIdByMovementId : IRequest<Guid>
    {
        public GetNotificationIdByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }

        public Guid MovementId { get; private set; }
    }
}