namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Exporter : Entity
    {
        public Exporter(string name, string type,
            Address address, Contact contact,
            string companyHouseNumber = null,
            string registrationNumber1 = null,
            string registrationNumber2 = null)
        {
            Guard.ArgumentNotNull(name);
            Guard.ArgumentNotNull(type);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(contact);

            // TODO - company type should be an enum, not a magic string. See WasteAction.
            if (String.Equals(type, "Limited Company", StringComparison.OrdinalIgnoreCase))
            {
                Guard.ArgumentNotNull(companyHouseNumber);
            }

            Name = name;
            Type = type;
            CompanyHouseNumber = companyHouseNumber;
            RegistrationNumber1 = registrationNumber1;
            RegistrationNumber2 = registrationNumber2;
            Address = address;
            Contact = contact;
        }

        private Exporter()
        {
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public string CompanyHouseNumber { get; private set; }

        public string RegistrationNumber1 { get; private set; }

        public string RegistrationNumber2 { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }
    }
}