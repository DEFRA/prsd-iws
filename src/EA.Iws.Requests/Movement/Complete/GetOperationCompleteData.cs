namespace EA.Iws.Requests.Movement.Complete
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetOperationCompleteData : IRequest<OperationCompleteData>
    {
        public Guid MovementId { get; private set; }

        public GetOperationCompleteData(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
