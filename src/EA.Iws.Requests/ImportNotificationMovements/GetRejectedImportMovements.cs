namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetRejectedImportMovements : IRequest<IList<RejectedMovementListData>>
    {
        public Guid NotificationId { get; private set; }

        public GetRejectedImportMovements(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
