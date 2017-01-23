namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationMovements;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementsByNotificationId : IRequest<MovementTableData[]>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportMovementsByNotificationId(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
