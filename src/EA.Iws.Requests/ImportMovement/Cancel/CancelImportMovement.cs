namespace EA.Iws.Requests.ImportMovement.Cancel
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class CancelImportMovement : IRequest<bool>
    {
        public CancelImportMovement(Guid movementId)
        {
            MovementId = movementId;
        }

        public Guid MovementId { get; private set; }
    }
}