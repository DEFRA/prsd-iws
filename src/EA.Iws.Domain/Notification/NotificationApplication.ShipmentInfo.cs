namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasShipmentInfo
        {
            get { return ShipmentInfo != null; }
        }

        public void SetSpecialHandling(bool isSpecialHandling, string specialHandlingDetails)
        {
            if (ShipmentInfo == null)
            {
                throw new InvalidOperationException("Shipment info does not exist for this notification");
            }
            ShipmentInfo.IsSpecialHandling = isSpecialHandling;
            ShipmentInfo.SpecialHandlingDetails = specialHandlingDetails;
        }

        public void AddShipmentDatesAndQuantityInfo(DateTime startDate, DateTime endDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
        {
            if (ShipmentInfo == null)
            {
                throw new InvalidOperationException("Shipment info does not exist for this notification");
            }

            ShipmentInfo.FirstDate = startDate;
            ShipmentInfo.LastDate = endDate;
            ShipmentInfo.Quantity = quantity;
            ShipmentInfo.Units = unit;
            ShipmentInfo.NumberOfShipments = numberOfShipments;
        }

        public void AddPackagingInfo(PackagingType packagingType, string otherDescription = null)
        {
            if (ShipmentInfo == null)
            {
                throw new InvalidOperationException("Shipment info does not exist for this notification");
            }
            ShipmentInfo.AddPackagingInfo(packagingType, otherDescription);
        }
    }
}