namespace EA.Iws.Requests.Movement.Receive
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetSubmittedMovements : IRequest<List<SubmittedMovement>>
    {
        public Guid NotificationId { get; private set; }

        public GetSubmittedMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
