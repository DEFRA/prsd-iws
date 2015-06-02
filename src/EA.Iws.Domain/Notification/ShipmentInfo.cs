namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class ShipmentInfo : Entity
    {
        private DateTime firstDate;
        private DateTime lastDate;

        internal ShipmentInfo()
        {
            PackagingInfosCollection = new List<PackagingInfo>();
            Units = ShipmentQuantityUnits.Tonnes;
        }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public bool IsSpecialHandling { get; internal set; }

        public string SpecialHandlingDetails { get; internal set; }

        public int NumberOfShipments { get; internal set; }

        public DateTime FirstDate
        {
            get { return firstDate; }
            set
            {
                if (value >= LastDate && LastDate != DateTime.MinValue)
                {
                    throw new InvalidOperationException("The start date must be before the end date");
                }
                firstDate = value;
            }
        }

        public DateTime LastDate
        {
            get { return lastDate; }
            set
            {
                if (value <= FirstDate && FirstDate != DateTime.MinValue)
                {
                    throw new InvalidOperationException("The end date must be after the start date");
                }
                lastDate = value;
            }
        }

        public ShipmentQuantityUnits Units { get; internal set; }

        public decimal Quantity { get; internal set; }

        public void AddPackagingInfo(PackagingType packagingType, string otherDescription = null)
        {
            var packagingInfo = new PackagingInfo(packagingType);
            if (!string.IsNullOrEmpty(otherDescription))
            {
                packagingInfo.OtherDescription = otherDescription;
            }
            PackagingInfosCollection.Add(packagingInfo);
        }
    }
}
