namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableProducer : Producer
    {
        public new Business Business 
        {
            get { return base.Business; }
            set { ObjectInstantiator<Producer>.SetProperty(x => x.Business, value, this); }
        }

        public new Contact Contact
        {
            get { return base.Contact; }
            set { ObjectInstantiator<Producer>.SetProperty(x => x.Contact, value, this); }
        }

        public new Address Address
        {
            get { return base.Address; }
            set { ObjectInstantiator<Producer>.SetProperty(x => x.Address, value, this); }
        }
    }
}
