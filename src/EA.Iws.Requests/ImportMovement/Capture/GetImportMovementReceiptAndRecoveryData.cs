namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementReceiptAndRecoveryData : IRequest<ImportMovementSummaryData>
    {
        public Guid ImportMovementId { get; private set; }

        public GetImportMovementReceiptAndRecoveryData(Guid importMovementId)
        {
            ImportMovementId = importMovementId;
        }
    }
}
