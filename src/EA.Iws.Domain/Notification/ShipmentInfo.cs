namespace EA.Iws.Domain.Notification
{
    using System.Collections.Generic;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class ShipmentInfo : Entity
    {
        protected ShipmentInfo()
        {
        }

        internal ShipmentInfo(bool isSpecialHandling)
        {
            IsSpecialHandling = false;
            PackagingInfosCollection = new List<PackagingInfo>();
        }

        public NumberOfShipmentsInfo NumberOfShipmentsInfo { get; set; }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public bool IsSpecialHandling { get; internal set; }

        public string SpecialHandlingDetails { get; set; }

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
