namespace EA.Iws.Domain.Notification
{
    using System;
    using Core.Domain;
    using Utils;

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

            this.Name = name;
            this.Address = address;
            this.Type = type;
            this.Contact = contact;
            this.NotificationId = notificationId;
            this.CompaniesHouseNumber = companiesHouseNumber;
            this.RegistrationNumber1 = registrationNumber1;
            this.RegistrationNumber2 = registrationNumber2;
            this.IsSiteOfExport = isSiteOfExport;
        }

        private Producer()
        {
        }
    }
}
