namespace EA.Iws.Requests.IntendedShipments
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetIntendedShipmentInfoForNotification : IRequest<Guid>
    {
        public SetIntendedShipmentInfoForNotification(Guid notificationId,
            int numberOfShipments,
            decimal quantity,
            ShipmentQuantityUnits units,
            DateTime startDate,
            DateTime endDate)
        {
            NotificationId = notificationId;
            NumberOfShipments = numberOfShipments;
            Quantity = quantity;
            Units = units;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }

        public decimal Quantity { get; private set; }

        public ShipmentQuantityUnits Units { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }
    }
}