namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Core.MovementReceipt;
    using Domain;
    using Domain.Movement;
    using Helpers;

    public class TestableMovementReceipt : MovementReceipt
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<MovementReceipt>.SetProperty(x => x.Id, value, this); }
        }

        public new ShipmentQuantity QuantityReceived
        {
            get { return base.QuantityReceived; }
            set { ObjectInstantiator<MovementReceipt>.SetProperty(x => x.QuantityReceived, value, this); }
        }

        public new DateTime Date
        {
            get { return base.Date; }
            set { ObjectInstantiator<MovementReceipt>.SetProperty(x => x.Date, value, this); }
        }

        public new Decision? Decision
        {
            get { return base.Decision; }
            set { ObjectInstantiator<MovementReceipt>.SetProperty(x => x.Decision, value, this); }
        }

        public new string RejectReason
        {
            get { return base.RejectReason; }
            set { ObjectInstantiator<MovementReceipt>.SetProperty(x => x.RejectReason, value, this); }
        }
    }
}