namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class ShipmentInfo : Entity
    {
        private int? numberOfShipments;
        private decimal? quantity;

        internal ShipmentInfo()
        {
            PackagingInfosCollection = new List<PackagingInfo>();
            Units = ShipmentQuantityUnits.NotSet;
        }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public bool IsSpecialHandling { get; internal set; }

        public string SpecialHandlingDetails { get; internal set; }

        public int? NumberOfShipments
        {
            get { return numberOfShipments; }
            internal set
            {
                if (value.HasValue)
                {
                    Guard.ArgumentNotZeroOrNegative(() => value.Value, value.Value);
                }
                numberOfShipments = value;
            }
        }

        public DateTime FirstDate { get; private set; }

        public DateTime LastDate { get; private set; }

        public ShipmentQuantityUnits Units { get; internal set; }

        public decimal? Quantity
        {
            get { return quantity; }
            internal set
            {
                if (value.HasValue)
                {
                    Guard.ArgumentNotZeroOrNegative(() => value.Value, value.Value);
                    quantity = decimal.Round(value.Value, 4, MidpointRounding.AwayFromZero);
                }
                else
                {
                    quantity = null;
                }
            }
        }

        public void AddPackagingInfo(PackagingType packagingType, string otherDescription = null)
        {
            var packagingInfo = new PackagingInfo(packagingType);
            if (!string.IsNullOrEmpty(otherDescription))
            {
                packagingInfo.OtherDescription = otherDescription;
            }
            PackagingInfosCollection.Add(packagingInfo);
        }

        internal void UpdateShipmentDates(DateTime firstDate, DateTime lastDate)
        {
            Guard.ArgumentNotDefaultValue(() => firstDate, firstDate);
            Guard.ArgumentNotDefaultValue(() => lastDate, lastDate);

            if (firstDate > lastDate)
            {
                throw new InvalidOperationException(
                    string.Format("The start date must be before the end date on shipment info {0}", Id));
            }

            FirstDate = firstDate;
            LastDate = lastDate;
        }
    }
}