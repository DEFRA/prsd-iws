namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementNumberByMovementId : IRequest<int>
    {
        public Guid MovementId { get; set; }

        public GetMovementNumberByMovementId(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
