namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetCancellableMovements : IRequest<IEnumerable<ImportCancellableMovement>>
    {
        public GetCancellableMovements(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }
    }
}