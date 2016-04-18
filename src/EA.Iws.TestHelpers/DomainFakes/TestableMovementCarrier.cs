namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Helpers;

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

        public static readonly TestableMovementCarrier MikeMerrysMovers = new TestableMovementCarrier
        {
            Id = new Guid("305382C0-C98D-41D7-B0F8-6BFED1DF0AAD"),
            Order = 0,
            Carrier = new TestableCarrier
            {
                Id = new Guid("CEE28232-96EB-4D38-B991-0C18CBAAB530"),
                Address = TestableAddress.SouthernHouse,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.MikeMerry
            }
        };
    }
}