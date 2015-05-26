namespace EA.Iws.Domain.Notification
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Exporter : Entity
    {
        private Business business;
        private Address address;
        private Contact contact;

        internal Exporter(Business business, Address address, Contact contact)
        {
            Business = business;
            Address = address;
            Contact = contact;
        }

        protected Exporter()
        {
        }

        public Business Business
        {
            get { return business; }
            set
            {
                Guard.ArgumentNotNull(value);
                business = value;
            }
        }

        public Address Address
        {
            get { return address; }
            set
            {
                Guard.ArgumentNotNull(value);
                address = value;
            }
        }

        public Contact Contact
        {
            get { return contact; }
            set
            {
                Guard.ArgumentNotNull(value);
                contact = value;
            }
        }
    }
}