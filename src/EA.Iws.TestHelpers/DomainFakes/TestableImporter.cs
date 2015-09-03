namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableImporter : Importer
    {
        public new Business Business 
        {
            get { return base.Business; }
            set { ObjectInstantiator<Importer>.SetProperty(x => x.Business, value, this); }
        }

        public new Contact Contact
        {
            get { return base.Contact; }
            set { ObjectInstantiator<Importer>.SetProperty(x => x.Contact, value, this); }
        }

        public new Address Address
        {
            get { return base.Address; }
            set { ObjectInstantiator<Importer>.SetProperty(x => x.Address, value, this); }
        }
    }
}
