namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetShipmentUnits : IRequest<ShipmentQuantityUnits>
    {
        public Guid NotificationId { get; private set; }

        public GetShipmentUnits(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}