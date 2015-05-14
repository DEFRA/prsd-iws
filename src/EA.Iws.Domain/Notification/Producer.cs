namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Producer : Entity
    {
        public virtual bool IsSiteOfExport { get; private set; }

        public BusinessNameAndType BusinessNameAndType { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public Producer(BusinessNameAndType businessNameAndType, Address address, Contact contact, bool isSiteOfExport)
        {
            Guard.ArgumentNotNull(businessNameAndType);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(contact);

            BusinessNameAndType = businessNameAndType;
            Address = address;
            Contact = contact;
            IsSiteOfExport = isSiteOfExport;
        }

        private Producer()
        {
        }
    }
}
