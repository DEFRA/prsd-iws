namespace EA.Iws.Requests.Movement.Summary
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetMovementReceiptAndRecoveryData : IRequest<MovementReceiptAndRecoveryData>
    {
        public Guid MovementId { get; private set; }

        public GetMovementReceiptAndRecoveryData(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
