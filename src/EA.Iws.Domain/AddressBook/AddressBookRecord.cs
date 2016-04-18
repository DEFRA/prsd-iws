namespace EA.Iws.Domain.AddressBook
{
    using NotificationApplication;
    using Prsd.Core.Domain;

    public class AddressBookRecord : Entity
    {
        public Address Address { get; private set; }

        public Business Business { get; private set; }

        public Contact Contact { get; private set; }

        protected AddressBookRecord()
        {
        }

        public AddressBookRecord(Address address, 
            Business business,
            Contact contact)
        {
            Address = address;
            Business = business;
            Contact = contact;
        }
    }
}