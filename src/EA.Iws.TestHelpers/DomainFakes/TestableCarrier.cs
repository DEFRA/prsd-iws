namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableCarrier : Carrier
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<Carrier>.SetProperty(x => x.Id, value, this); }
        }
        public new Business Business
        {
            get { return base.Business; }
            set { ObjectInstantiator<Carrier>.SetProperty(x => x.Business, value, this); }
        }

        public new Contact Contact
        {
            get { return base.Contact; }
            set { ObjectInstantiator<Carrier>.SetProperty(x => x.Contact, value, this); }
        }

        public new Address Address
        {
            get { return base.Address; }
            set { ObjectInstantiator<Carrier>.SetProperty(x => x.Address, value, this); }
        }
    }
}
