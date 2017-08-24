namespace EA.Iws.Requests.Movement
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using System;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class DoesMovementDetailsExist : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public DoesMovementDetailsExist(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
