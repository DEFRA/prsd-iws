namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Exporter : Entity
    {
        protected Exporter()
        {
        }

        public Exporter(Guid importNotificationId, string businessName, Address address, Contact contact, BusinessType businessType, string registrationNumber)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
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

        public string RegistrationNumber { get; set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public void UpdateExporterDetails(Contact contact, string businessName, Address address)
        {
            Guard.ArgumentNotNull(() => contact, contact);
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
            Guard.ArgumentNotNull(() => address, address);
            Contact = contact;
            Name = businessName;
            Address = address;
        }
    }
}