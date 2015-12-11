namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Facility : Entity
    {
        protected Facility()
        {    
        }

        public Facility(string businessName, BusinessType businessType,
            string registrationNumber,
            Address address, Contact contact, bool isActualSiteOfTreatment)
        {
            Guard.ArgumentNotNullOrEmpty(() => businessName, businessName);
            Guard.ArgumentNotDefaultValue(() => businessType, businessType);
            Guard.ArgumentNotNullOrEmpty(() => registrationNumber, registrationNumber);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            Name = businessName;
            Type = businessType;
            RegistrationNumber = registrationNumber;
            Address = address;
            Contact = contact;
            IsActualSiteOfTreatment = isActualSiteOfTreatment;
        }

        public string Name { get; private set; }

        public BusinessType Type { get; private set; }

        public string RegistrationNumber { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public bool IsActualSiteOfTreatment { get; private set; }
    }
}