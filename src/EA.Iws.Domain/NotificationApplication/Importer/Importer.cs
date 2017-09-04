namespace EA.Iws.Domain.NotificationApplication.Importer
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Importer : Entity
    {
        protected Importer()
        {
        }

        public Importer(Guid notificationId, Address address, Business business, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            Business = business;
            Address = address;
            Contact = contact;
            NotificationId = notificationId;
        }

        public Business Business { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public Guid NotificationId { get; private set; }

        public void Update(Address address, Business business, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            Business = business;
            Address = address;
            Contact = contact;
        }

        public void UpdateContact(Contact contact)
        {
            Guard.ArgumentNotNull(() => contact, contact);
            Contact = contact;
        }
    }
}