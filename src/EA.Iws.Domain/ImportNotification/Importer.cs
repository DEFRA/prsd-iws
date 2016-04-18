namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Importer : Entity
    {
        protected Importer()
        {    
        }

        public Importer(Guid importNotificationId, string businessName, BusinessType businessType,
            string registrationNumber,
            Address address, Contact contact)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
            Guard.ArgumentNotDefaultValue(() => businessType, businessType);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            ImportNotificationId = importNotificationId;
            Name = businessName;
            Type = businessType;
            RegistrationNumber = registrationNumber;
            Address = address;
            Contact = contact;
        }

        public Guid ImportNotificationId { get; private set; }

        public string Name { get; private set; }

        public BusinessType Type { get; private set; }

        public string RegistrationNumber { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }
    }
}