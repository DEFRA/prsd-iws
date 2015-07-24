namespace EA.Iws.Domain.NotificationApplication
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasShipmentInfo
        {
            get { return ShipmentInfo != null; }
        }

        public void SetShipmentInfo(DateTime firstDate, DateTime lastDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
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
                ShipmentInfo = new ShipmentInfo(firstDate, lastDate, numberOfShipments, quantity, unit);
            }
            else
            {
                ShipmentInfo.UpdateShipmentPeriod(firstDate, lastDate);
                ShipmentInfo.UpdateQuantity(quantity, unit);
                ShipmentInfo.NumberOfShipments = numberOfShipments;
            }
        }
    }
}