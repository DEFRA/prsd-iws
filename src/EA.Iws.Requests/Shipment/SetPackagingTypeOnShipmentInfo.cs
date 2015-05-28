namespace EA.Iws.Requests.Shipment
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class SetPackagingTypeOnShipmentInfo : IRequest<Guid>
    {
        public SetPackagingTypeOnShipmentInfo(List<PackagingType> packagingTypes, Guid notificationId)
        {
            PackagingTypes = packagingTypes;

            NotificationId = notificationId;
        }

        public List<PackagingType> PackagingTypes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}