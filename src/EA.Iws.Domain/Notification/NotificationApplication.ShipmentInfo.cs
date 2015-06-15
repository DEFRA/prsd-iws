namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasShipmentInfo
        {
            get { return ShipmentInfo != null; }
        }

        public void AddShipmentInfo(DateTime startDate, DateTime endDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
        {
            if (ShipmentInfo != null)
            {
                throw new InvalidOperationException(string.Format("ShipmentInfo already exists, can't add another to this notification: {0}", Id));
            }

            int monthPeriodLength = IsPreconsentedRecoveryFacility.GetValueOrDefault() ? 36 : 12;
            if (endDate > startDate.AddMonths(monthPeriodLength))
            {
                throw new InvalidOperationException(string.Format("The start date and end date must be within a {0} month period for this notification {1}", monthPeriodLength, Id));
            }

            ShipmentInfo = new ShipmentInfo(startDate, endDate, numberOfShipments, quantity, unit);
        }
    }
}