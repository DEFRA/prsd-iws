namespace EA.Iws.Requests.Shipment
{
    using System;
    using Core.Shipment;
    using Prsd.Core.Mediator;

    public class SetShipmentInfoForNotification : IRequest<Guid>
    {
        public SetShipmentInfoForNotification(Guid notificationId, int numberOfShipments,  decimal quantity, ShipmentQuantityUnits units, DateTime startDate, DateTime endDate)
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