namespace EA.Iws.Requests.Movement.Reject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetRejectionByMovementId : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public GetRejectionByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
