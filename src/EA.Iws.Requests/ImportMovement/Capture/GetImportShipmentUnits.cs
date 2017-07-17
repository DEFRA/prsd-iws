namespace EA.Iws.Requests.ImportMovement.Capture
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanReadImportMovements)]
   public class GetImportShipmentUnits : IRequest<ShipmentQuantityUnits>
    {
        public Guid NotificationId { get; private set; }

        public GetImportShipmentUnits(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
