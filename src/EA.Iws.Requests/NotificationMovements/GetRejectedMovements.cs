namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetRejectedMovements : IRequest<IList<RejectedMovementListData>>
    {
        public Guid NotificationId { get; private set; }

        public GetRejectedMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
