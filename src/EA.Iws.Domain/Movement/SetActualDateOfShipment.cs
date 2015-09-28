namespace EA.Iws.Domain.Movement
{
    using System;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;

    public class SetActualDateOfShipment
    {
        public void Apply(DateTime date, Movement movement, ShipmentInfo shipmentInfo)
        {
            if (movement.NotificationId != shipmentInfo.NotificationId)
            {
                throw new InvalidOperationException(string.Format(
                    "Movement and ShipmentInfo should have the same parent Notification. MovementId: {0} ShipmentId: {1}",
                    movement.NotificationId,
                    shipmentInfo.NotificationId));
            }

            if (!shipmentInfo.ShipmentPeriod.IsDateInShipmentPeriod(date))
            {
                throw new InvalidOperationException(
                    "The date is not within the shipment date range for this notification " + date);
            }

            movement.Date = date;
        }
    }
}
