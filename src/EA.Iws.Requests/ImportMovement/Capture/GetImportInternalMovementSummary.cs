namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportInternalMovementSummary : IRequest<ImportInternalMovementSummary>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportInternalMovementSummary(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
