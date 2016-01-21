namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementDates : IRequest<ImportMovementData>
    {
        public Guid MovementId { get; private set; }

        public GetImportMovementDates(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
