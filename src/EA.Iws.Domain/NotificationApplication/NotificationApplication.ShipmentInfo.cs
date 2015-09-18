namespace EA.Iws.Domain.NotificationApplication
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasShipmentInfo
        {
            get { return ShipmentInfo != null; }
        }

        public void SetShipmentInfo(DateTime firstDate, DateTime lastDate, int numberOfShipments, ShipmentQuantity shipmentQuantity)
        {
            int monthPeriodLength = IsPreconsentedRecoveryFacility.GetValueOrDefault() ? 36 : 12;
            if (lastDate >= firstDate.AddMonths(monthPeriodLength))
            {
                throw new InvalidOperationException(
                    string.Format("The start date and end date must be within a {0} month period for this notification {1}",
                        monthPeriodLength, Id));
            }

            if (ShipmentInfo == null)
            {
                ShipmentInfo = new ShipmentInfo(firstDate, lastDate, numberOfShipments, shipmentQuantity);
            }
            else
            {
                ShipmentInfo.UpdateShipmentPeriod(firstDate, lastDate);
                ShipmentInfo.UpdateQuantity(shipmentQuantity);
                ShipmentInfo.NumberOfShipments = numberOfShipments;
            }
        }
    }
}