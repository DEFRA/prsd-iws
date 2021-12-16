namespace EA.Iws.Requests.ImportMovement.PartialReject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class GetImportPartialRejectionByMovementId : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public GetImportPartialRejectionByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
