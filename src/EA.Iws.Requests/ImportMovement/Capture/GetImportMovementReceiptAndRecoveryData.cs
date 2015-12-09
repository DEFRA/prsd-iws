namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    public class GetImportMovementReceiptAndRecoveryData : IRequest<ImportMovement>
    {
        public Guid ImportMovementId { get; private set; }

        public GetImportMovementReceiptAndRecoveryData(Guid importMovementId)
        {
            ImportMovementId = importMovementId;
        }
    }
}
