namespace EA.Iws.Domain.NotificationApplication.Exporter
{
    using Prsd.Core;
    using Prsd.Core.Domain;
    using System;

    public class Exporter : Entity
    {
        protected Exporter()
        {
        }

        public Exporter(Guid notificationId, bool isUkBased, Address address, Business business, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            IsUkBased = isUkBased;
            Business = business;
            Address = address;
            Contact = contact;
            NotificationId = notificationId;
        }

        public bool IsUkBased { get; private set; }

        public Business Business { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public Guid NotificationId { get; private set; }

        public void Update(bool isUkBased, Address address, Business business, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            IsUkBased = isUkBased;
            Business = business;
            Address = address;
            Contact = contact;
        }

        public void UpdateContactAndBusiness(Contact contact, Business business, Address address)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => contact, contact);
            Guard.ArgumentNotNull(() => address, address);

            Business = business;
            Contact = contact;
            Address = address;
        }
    }
}