namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementDateByMovementId : IRequest<DateTime>
    {
        public Guid MovementId { get; private set; }

        public GetMovementDateByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
