namespace EA.Iws.Requests.ImportMovement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
    public class GetImportMovementAuditByNotificationId : IRequest<ShipmentAuditData>
    {
        public Guid NotificationId { get; private set; }

        public int PageNumber { get; private set; }

        public int? ShipmentNumber { get; private set; }

        public GetImportMovementAuditByNotificationId(Guid notificationId, int pageNumber, int? shipmentNumber = null)
        {
            NotificationId = notificationId;
            PageNumber = pageNumber;
            ShipmentNumber = shipmentNumber;
        }
    }
}
