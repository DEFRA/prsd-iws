namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetRejectedMovementDetails : IRequest<RejectedMovementDetails>
    {
        public Guid MovementId { get; private set; }

        public int Number { get; private set; }

        public GetRejectedMovementDetails(Guid movementId, int number)
        {
            MovementId = movementId;
            Number = number;
        }
    }
}
