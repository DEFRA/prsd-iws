namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasShipmentInfo
        {
            get { return ShipmentInfo != null; }
        }

        public void AddShipmentInfo(DateTime startDate, DateTime endDate, int numberOfShipments, decimal quantity,
            ShipmentQuantityUnits unit)
        {
            if (ShipmentInfo != null)
            {
                throw new InvalidOperationException(
                    String.Format("Cannot add shipment info to notification: {0} if it already exists", Id));
            }

            ShipmentInfo = new ShipmentInfo(numberOfShipments, quantity, unit, startDate, endDate);
        }

        public void SetSpecialHandling(bool isSpecialHandling, string specialHandlingDetails)
        {
            if (ShipmentInfo == null)
            {
                throw new InvalidOperationException("Shiping info does not exist for this notification");
            }

            ShipmentInfo.IsSpecialHandling = isSpecialHandling;
            ShipmentInfo.SpecialHandlingDetails = specialHandlingDetails;
        }
    }
}