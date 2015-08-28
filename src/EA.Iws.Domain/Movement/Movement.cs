namespace EA.Iws.Domain.Movement
{
    using System;
    using NotificationApplication;
    using System.Collections.Generic;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class Movement : Entity
    {
        protected Movement()
        {
        }

        internal Movement(NotificationApplication notificationApplication, int movementNumber)
        {
            NotificationApplicationId = notificationApplication.Id;
            Number = movementNumber;
        }

        public int Number { get; private set; }

        public virtual NotificationApplication NotificationApplication { get; private set; }

        public DateTime Date { get; private set; }

        public Guid NotificationApplicationId { get; private set; }

        public decimal Quantity { get; private set; }

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
