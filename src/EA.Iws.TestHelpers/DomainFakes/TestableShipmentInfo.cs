namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableShipmentInfo : ShipmentInfo
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.Id, value, this); }
        }

        public new Guid NotificationId
        {
            get { return base.Id; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.NotificationId, value, this); }
        }

        public new int NumberOfShipments
        {
            get { return base.NumberOfShipments; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.NumberOfShipments, value, this); }
        }

        public new decimal Quantity
        {
            get { return base.Quantity; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.Quantity, value, this); }
        }

        public new ShipmentQuantityUnits Units
        {
            get { return base.Units; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.Units, value, this); }
        }

        public new ShipmentPeriod ShipmentPeriod
        {
            get { return base.ShipmentPeriod; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.ShipmentPeriod, value, this); }
        }
    }
}
