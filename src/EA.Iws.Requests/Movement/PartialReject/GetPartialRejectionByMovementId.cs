namespace EA.Iws.Requests.Movement.PartialReject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetPartialRejectionByMovementId : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public GetPartialRejectionByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
