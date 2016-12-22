namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetRejectedImportMovementDetails : IRequest<RejectedMovementDetails>
    {
        public Guid MovementId { get; private set; }

        public GetRejectedImportMovementDetails(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
