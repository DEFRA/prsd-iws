namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Carrier : Entity
    {
        private Address address;
        private Business business;
        private Contact contact;

        internal Carrier(Business business, Address address, Contact contact)
        {
            Business = business;
            Address = address;
            Contact = contact;
        }

        protected Carrier()
        {
        }

        public Business Business
        {
            get { return business; }
            set
            {
                Guard.ArgumentNotNull(() => value, value);
                business = value;
            }
        }

        public Address Address
        {
            get { return address; }
            set
            {
                Guard.ArgumentNotNull(() => value, value);
                address = value;
            }
        }

        public Contact Contact
        {
            get { return contact; }
            set
            {
                Guard.ArgumentNotNull(() => value, value);
                contact = value;
            }
        }
    }
}