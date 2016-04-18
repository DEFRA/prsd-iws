namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Facility : Entity
    {
        private Business business;
        private Address address;
        private Contact contact;

        protected Facility()
        {
        }

        internal Facility(Business business, Address address, Contact contact)
        {
            Business = business;
            Address = address;
            Contact = contact;
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

        public bool IsActualSiteOfTreatment { get; internal set; }
    }
}