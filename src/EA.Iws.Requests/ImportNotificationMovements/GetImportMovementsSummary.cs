namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationMovements;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementsSummary : IRequest<Summary>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportMovementsSummary(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
