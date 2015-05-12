namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Producer : Entity
    {
        public string Name { get; private set; }

        public Address Address { get; private set; }

        public Contact Contact { get; private set; }

        public Guid NotificationId { get; set; }

        public string Type { get; private set; }

        public string CompaniesHouseNumber { get; private set; }

        public string RegistrationNumber1 { get; private set; }

        public string RegistrationNumber2 { get; private set; }

        public virtual bool IsSiteOfExport { get; private set; }

        public Producer(string name, Address address, Contact contact, Guid notificationId, bool isSiteOfExport, string type,
                        string companiesHouseNumber = null, string registrationNumber1 = null, string registrationNumber2 = null)
        {
            Guard.ArgumentNotNull(name);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(type);
            Guard.ArgumentNotNull(contact);

            Name = name;
            Address = address;
            Type = type;
            Contact = contact;
            NotificationId = notificationId;
            CompaniesHouseNumber = companiesHouseNumber;
            RegistrationNumber1 = registrationNumber1;
            RegistrationNumber2 = registrationNumber2;
            IsSiteOfExport = isSiteOfExport;
        }

        private Producer()
        {
        }
    }
}
