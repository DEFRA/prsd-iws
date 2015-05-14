namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Exporter : Entity
    {
        public BusinessNameAndType BusinessNameAndType { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public Exporter(BusinessNameAndType businessNameAndType, Address address, Contact contact)
        {
            Guard.ArgumentNotNull(businessNameAndType);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(contact);

            BusinessNameAndType = businessNameAndType;
            Address = address;
            Contact = contact;
        }

        private Exporter()
        {
        }
    }
}