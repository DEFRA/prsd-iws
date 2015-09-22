namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableShipmentInfo : ShipmentInfo
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.Id, value, this); }
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

        public new DateTime FirstDate
        {
            get { return base.FirstDate; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.FirstDate, value, this); }
        }

        public new DateTime LastDate
        {
            get { return base.LastDate; }
            set { ObjectInstantiator<ShipmentInfo>.SetProperty(x => x.LastDate, value, this); }
        }
    }
}
