namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementsByIds : IRequest<MovementInfo[]>
    {
        public GetMovementsByIds(Guid notificationId, IEnumerable<Guid> movementIds)
        {
            NotificationId = notificationId;
            MovementIds = movementIds;
        }

        public Guid NotificationId { get; private set; }

        public IEnumerable<Guid> MovementIds { get; private set; }
    }
}