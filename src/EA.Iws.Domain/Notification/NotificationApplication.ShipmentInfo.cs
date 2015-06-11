namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;

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
                throw new InvalidOperationException(string.Format("Shipment info does not exist for this notification: {0}", Id));
            }
            ShipmentInfo.IsSpecialHandling = isSpecialHandling;
            ShipmentInfo.SpecialHandlingDetails = specialHandlingDetails;
        }

        public void AddShipmentDatesAndQuantityInfo(DateTime startDate, DateTime endDate, int numberOfShipments, decimal quantity, ShipmentQuantityUnits unit)
        {
            if (ShipmentInfo == null)
            {
                throw new InvalidOperationException(string.Format("Shipment info does not exist for this notification: {0}", Id));
            }

            int monthPeriodLength = (IsPreconsentedRecoveryFacility.HasValue && IsPreconsentedRecoveryFacility.Value) ? 36 : 12;
            if (endDate > startDate.AddMonths(monthPeriodLength))
            {
                throw new InvalidOperationException(string.Format("The start date and end date must be within a {0} month period for this notification {1}", monthPeriodLength, Id));
            }

            ShipmentInfo.UpdateShipmentDates(startDate, endDate);
            ShipmentInfo.Quantity = quantity;
            ShipmentInfo.Units = unit;
            ShipmentInfo.NumberOfShipments = numberOfShipments;
        }

        public void UpdatePackagingInfo(IEnumerable<PackagingInfo> packagingInfos)
        {
            if (ShipmentInfo == null)
            {
                ShipmentInfo = new ShipmentInfo();
            }

            ShipmentInfo.UpdatePackagingInfo(packagingInfos);
        }
    }
}