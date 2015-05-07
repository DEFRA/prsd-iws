namespace EA.Iws.Cqrs.ExporterNotifier
{
    using System;
    using Core.Cqrs;

    public class CreateExporter : ICommand
    {
        public Guid NotificationId { get; private set; }
        public string Type { get; private set; }
        public string CompanyHouseNumber { get; private set; }
        public string RegistrationNumber1 { get; private set; }
        public string RegistrationNumber2 { get; private set; }
        public string Name { get; private set; }

        public string Building { get; private set; }
        public string Address1 { get; private set; }
        public string Address2 { get; private set; }
        public string City { get; private set; }
        public string County { get; private set; }
        public string PostCode { get; private set; }
        public Guid CountryId { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Fax { get; private set; }
        public string Email { get; private set; }

        public CreateExporter(Guid notificationId, string name, string type, string registrationNumber1, string registrationNumber2, string companyHouseNumber,
            string building, string address1, string address2, string city, string county, string postCode, string countryId,
            string contactFirstName, string contactLastName, string phone, string fax, string email)
        {
            //Exporter company
            NotificationId = notificationId;
            CompanyHouseNumber = companyHouseNumber;
            RegistrationNumber1 = registrationNumber1;
            RegistrationNumber2 = registrationNumber2;
            Type = type;
            Name = name;

            //Exporter Address
            Building = building;
            Address1 = address1;
            Address2 = address2;
            County = county;
            City = city;
            PostCode = postCode;
            CountryId = Guid.Parse(countryId);

            //Exporter Contact
            FirstName = contactFirstName;
            LastName = contactLastName;
            Phone = phone;
            Fax = fax;
            Email = email;
        }
    }
}
