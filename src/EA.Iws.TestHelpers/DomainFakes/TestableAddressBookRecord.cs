namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.AddressBook;
    using Helpers;

    public class TestableAddressBookRecord : AddressBookRecord
    {
        public new Address Address
        {
            get { return base.Address; }
            set { ObjectInstantiator<AddressBookRecord>.SetProperty(x => x.Address, value, this); }
        }

        public new Business Business
        {
            get { return base.Business; }
            set { ObjectInstantiator<AddressBookRecord>.SetProperty(x => x.Business, value, this); }
        }

        public new Contact Contact
        {
            get { return base.Contact; }
            set { ObjectInstantiator<AddressBookRecord>.SetProperty(x => x.Contact, value, this); }
        }
    }
}
