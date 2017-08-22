namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementDetailsByIds : IRequest<MovementBasicDetails>
    {
        public GetMovementDetailsByIds(Guid notificationId, Guid movementIds)
        {
            NotificationId = notificationId;
            MovementIds = movementIds;
        }

        public Guid NotificationId { get; private set; }

        public Guid MovementIds { get; private set; }
    }
}
