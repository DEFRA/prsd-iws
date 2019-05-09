namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetSubmittedPendingMovements : IRequest<List<SubmittedMovement>>
    {
        public Guid NotificationId { get; private set; }

        public GetSubmittedPendingMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
