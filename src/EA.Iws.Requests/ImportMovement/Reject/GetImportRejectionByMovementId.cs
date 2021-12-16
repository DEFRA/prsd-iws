namespace EA.Iws.Requests.ImportMovement.Reject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class GetImportRejectionByMovementId : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public GetImportRejectionByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
