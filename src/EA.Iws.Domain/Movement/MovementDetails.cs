namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;

    public class MovementDetails : Entity
    {
        protected MovementDetails()
        {
        }

        internal MovementDetails(Guid movementId,
            ShipmentQuantity actualQuantity,
            IEnumerable<PackagingInfo> packagingInfos)
        {
            Guard.ArgumentNotDefaultValue(() => movementId, movementId);
            Guard.ArgumentNotNull(() => actualQuantity, actualQuantity);
            Guard.ArgumentNotNull(() => packagingInfos, packagingInfos);

            if (!packagingInfos.Any())
            {
                throw new ArgumentException("Packaging infos can not be empty.", "packagingInfos");
            }

            if (actualQuantity <= new ShipmentQuantity(0, actualQuantity.Units))
            {
                throw new ArgumentException("Actual quantity must be greater than zero", "actualQuantity");
            }

            MovementId = movementId;
            ActualQuantity = actualQuantity;
            PackagingInfosCollection = packagingInfos.ToList();
        }

        public Guid MovementId { get; private set; }

        public ShipmentQuantity ActualQuantity { get; private set; }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        protected virtual ICollection<MovementCarrier> CarriersCollection { get; set; }

        public int? NumberOfPackages { get; private set; }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<MovementCarrier> Carriers
        {
            get { return CarriersCollection.ToSafeIEnumerable(); }
        }
    }
}