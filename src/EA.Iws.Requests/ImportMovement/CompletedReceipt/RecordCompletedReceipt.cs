namespace EA.Iws.Requests.ImportMovement.CompletedReceipt
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class RecordCompletedReceipt : IRequest<bool>
    {
        public Guid ImportMovementId { get; private set; }

        public DateTime Date { get; private set; }

        public RecordCompletedReceipt(Guid importMovementId, DateTime date)
        {
            ImportMovementId = importMovementId;
            Date = date;
        }
    }
}
