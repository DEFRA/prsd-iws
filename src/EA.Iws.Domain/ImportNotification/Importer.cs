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
            Guard.ArgumentNotNullOrEmpty(() => registrationNumber, registrationNumber);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            ImportNotificationId = importNotificationId;
            Name = businessName;
            Type = businessType;
            RegistrationNumber = registrationNumber;
            Address = address;
            Contact = contact;
        }

        public Guid ImportNotificationId { get; set; }

        public string Name { get; set; }

        public BusinessType Type { get; set; }

        public string RegistrationNumber { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }
    }
}