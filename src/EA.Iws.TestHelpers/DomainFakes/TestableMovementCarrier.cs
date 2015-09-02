namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Helpers;
using System;

    public class TestableMovementCarrier : MovementCarrier
    {
        public new Guid Id
        { 
            get { return base.Id; }
            set { ObjectInstantiator<MovementCarrier>.SetProperty(x => x.Id, value, this); }
        }

        public new Carrier Carrier
        { 
            get { return base.Carrier; }
            set { ObjectInstantiator<MovementCarrier>.SetProperty(x => x.Carrier, value, this); }
        }

        public new int Order
        {
            get { return base.Order; }
            set { ObjectInstantiator<MovementCarrier>.SetProperty(x => x.Order, value, this); }
        }
    }
}
