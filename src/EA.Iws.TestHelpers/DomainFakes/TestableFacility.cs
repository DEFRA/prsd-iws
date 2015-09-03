namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableFacility : Facility
    {
        public new Business Business 
        {
            get { return base.Business; }
            set { ObjectInstantiator<Facility>.SetProperty(x => x.Business, value, this); }
        }

        public new Contact Contact
        {
            get { return base.Contact; }
            set { ObjectInstantiator<Facility>.SetProperty(x => x.Contact, value, this); }
        }

        public new Address Address
        {
            get { return base.Address; }
            set { ObjectInstantiator<Facility>.SetProperty(x => x.Address, value, this); }
        }
    }
}
