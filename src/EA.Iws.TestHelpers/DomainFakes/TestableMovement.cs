namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableMovement : Movement
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Id, value, this); }
        }

        public new Guid NotificationApplicationId
        {
            get { return base.NotificationApplicationId; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.NotificationApplicationId, value, this); }
        }

        public new NotificationApplication NotificationApplication
        {
            get { return base.NotificationApplication; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.NotificationApplication, value, this); }
        }

        public new decimal? Quantity
        {
            get { return base.Quantity.GetValueOrDefault(); }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Quantity, value, this); }
        }

        public new ShipmentQuantityUnits? Units
        {
            get { return base.Units; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Units, value, this); }
        }

        public new DateTime? Date
        {
            get { return base.Date; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Date, value, this); }
        }

        public new IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return base.PackagingInfos; }
            set { PackagingInfosCollection = (value == null) ? null : value.ToList(); }
        }

        public new IEnumerable<MovementCarrier> MovementCarriers
        {
            get { return base.MovementCarriers; }
            set { MovementCarriersCollection = (value == null) ? null : value.ToList(); }
        }

        public new int? NumberOfPackages
        {
            get { return base.NumberOfPackages; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.NumberOfPackages, value, this); }
        }

        public new int Number
        {
            get { return base.Number; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Number, value, this); }
        }

        public new MovementReceipt Receipt
        {
            get { return base.Receipt; }
            set { ObjectInstantiator<Movement>.SetProperty(x => x.Receipt, value, this); }
        }
    }
}
