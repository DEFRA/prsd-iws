namespace EA.Iws.Domain.Movement
{
    using Core.Shared;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using System;
    using System.Collections.Generic;

    public class Movement : Entity
    {
        protected Movement()
        {
        }

        internal Movement(NotificationApplication notificationApplication, int movementNumber)
        {
            NotificationApplicationId = notificationApplication.Id;
            Number = movementNumber;
            NotificationApplication = notificationApplication;
        }

        public int Number { get; private set; }

        public virtual NotificationApplication NotificationApplication { get; private set; }

        public DateTime? Date { get; private set; }

        public Guid NotificationApplicationId { get; private set; }

        public decimal? Quantity { get; private set; }

        public ShipmentQuantityUnits? Units { get; private set; }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        public IEnumerable<PackagingInfo> PackagingInfos 
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public void UpdateDate(DateTime date)
        {
            if (date >= NotificationApplication.ShipmentInfo.FirstDate
                && date <= NotificationApplication.ShipmentInfo.LastDate)
            {
                this.Date = date;
            }
            else
            {
                throw new InvalidOperationException(
                    "The date is not within the shipment date range for this notification " + date);
            }
        }

        public void SetQuantity(decimal quantity, ShipmentQuantityUnits units)
        {
            Guard.ArgumentNotZeroOrNegative(() => quantity, quantity);
            Guard.ArgumentNotNull(() => NotificationApplication.ShipmentInfo, NotificationApplication.ShipmentInfo);

            var notificationUnits = NotificationApplication.ShipmentInfo.Units;

            if (units != notificationUnits)
            {
                quantity = ShipmentQuantityUnitConverter.ConvertToTarget(units, notificationUnits, quantity);
            }

            Quantity = quantity;
            Units = notificationUnits;
        }

        public void SetPackagingInfos(IEnumerable<PackagingInfo> packagingInfos)
        {
            PackagingInfosCollection.Clear();

            foreach (var packagingInfo in packagingInfos)
            {
                PackagingInfosCollection.Add(packagingInfo);
            }
        }
    }
}
