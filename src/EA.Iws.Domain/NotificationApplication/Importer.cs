namespace EA.Iws.Domain.Notification
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Importer : Entity
    {
        internal Importer(Business business, Address address, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            Business = business;
            Address = address;
            Contact = contact;
        }

        protected Importer()
        {
        }

        public Business Business { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }
    }
}