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
        public Guid MovementId { get; private set; }
        public ShipmentQuantity ActualQuantity { get; private set; }
        public int NumberOfPackages { get; private set; }
        protected virtual ICollection<MovementCarrier> CarriersCollection { get; set; }
        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        public IEnumerable<MovementCarrier> Carriers
        {
            get
            {
                return CarriersCollection.ToSafeIEnumerable();
            }
        }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get
            {
                return PackagingInfosCollection.ToSafeIEnumerable();
            }
        }

        protected MovementDetails()
        {
        }

        internal MovementDetails(Guid movementId,
            ShipmentQuantity actualQuantity,
            int numberOfPackages,
            IEnumerable<MovementCarrier> carriers,
            IEnumerable<PackagingInfo> packagingInfos)
        {
            Guard.ArgumentNotDefaultValue(() => movementId, movementId);
            Guard.ArgumentNotNull(() => actualQuantity, actualQuantity);
            Guard.ArgumentNotZeroOrNegative(() => numberOfPackages, numberOfPackages);

            Guard.ArgumentNotNull(() => carriers, carriers);
            Guard.ArgumentNotNull(() => packagingInfos, packagingInfos);

            if (!carriers.Any())
            {
                throw new ArgumentException("Parameter carriers can not be empty.");
            }

            if (!packagingInfos.Any())
            {
                throw new ArgumentException("Parameter packagingInfos can not be empty.");
            }

            MovementId = movementId;
            ActualQuantity = actualQuantity;
            NumberOfPackages = numberOfPackages;

            CarriersCollection = carriers.ToList();
            PackagingInfosCollection = packagingInfos.ToList();
        }
    }
}