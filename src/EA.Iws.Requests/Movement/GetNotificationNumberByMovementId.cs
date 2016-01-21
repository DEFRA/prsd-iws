namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetNotificationNumberByMovementId : IRequest<string>
    {
        public Guid MovementId { get; private set; }

        public GetNotificationNumberByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}