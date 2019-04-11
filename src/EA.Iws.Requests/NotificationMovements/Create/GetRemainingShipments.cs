namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanCreateExportMovements)]
    public class GetRemainingShipments : IRequest<RemainingShipmentsData>
    {
        public Guid NotificationId { get; private set; }

        public DateTime? ShipmentDate { get; private set; }

        public GetRemainingShipments(Guid notificationId, DateTime? shipmentDate = null)
        {
            NotificationId = notificationId;
            ShipmentDate = shipmentDate;
        }
    }
}