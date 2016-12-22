namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsInternal)]
    public class GetRejectedMovementDetails : IRequest<RejectedMovementDetails>
    {
        public Guid MovementId { get; private set; }

        public GetRejectedMovementDetails(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
