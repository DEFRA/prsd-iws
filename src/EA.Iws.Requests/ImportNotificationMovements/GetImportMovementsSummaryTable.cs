namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementsSummaryTable : IRequest<MovementsSummary>
    {
        public Guid ImportNotificationId { get; private set; }

        public ImportMovementStatus? Status { get; private set; }

        public GetImportMovementsSummaryTable(Guid importNotificationId, ImportMovementStatus? status)
        {
            ImportNotificationId = importNotificationId;
            Status = status;
        }
    }
}
