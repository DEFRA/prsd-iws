namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsExternal)]
    public class GetReceiptRecoveryMovementsByNotificationId : IRequest<IList<MovementData>>
    {
        public Guid NotificationId { get; private set; }

        public GetReceiptRecoveryMovementsByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
