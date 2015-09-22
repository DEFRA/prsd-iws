namespace EA.Iws.Domain.Movement
{
    using System;
    using Domain.NotificationApplication;

    public class SetActualDateOfShipment
    {
        public void Apply(DateTime date, Movement movement, ShipmentInfo shipmentInfo)
        {
            //TODO: When shipment info is decoupled from the notification, we will have the notification Id back.
            //if (movement.NotificationId != shipmentInfo.NotificationId)
            //{
            //    throw new InvalidOperationException(string.Format(
            //        "Movement and ShipmentInfo should have the same parent Notification. MovementId: {0} ShipmentId: {1}",
            //        movement.NotificationId,
            //        shipmentInfo.NotificationId));
            //}

            if (!(date >= shipmentInfo.FirstDate
                && date <= shipmentInfo.LastDate))
            {
                throw new InvalidOperationException(
                    "The date is not within the shipment date range for this notification " + date);
            }

            movement.Date = date;
        }
    }
}
