namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using EA.Iws.Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Producer : Entity
    {
        protected Producer()
        {        
        }
    
        public Producer(Guid importNotificationId, string businessName, Address address, Contact contact, bool isOnlyProducer, BusinessType businessType, string registrationNumber)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            ImportNotificationId = importNotificationId;
            Name = businessName;
            Address = address;
            Contact = contact;
            IsOnlyProducer = isOnlyProducer;
            Type = businessType;
            RegistrationNumber = registrationNumber;
        }

        public Guid ImportNotificationId { get; private set; }

        public string Name { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public bool IsOnlyProducer { get; private set; }

        public BusinessType Type { get; set; }

        public string RegistrationNumber { get; set; }
    }
}