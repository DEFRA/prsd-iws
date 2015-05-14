namespace EA.Iws.Requests.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateExporter : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public string Type { get; set; }

        public string CompaniesHouseNumber { get; set; }

        public string RegistrationNumber1 { get; set; }

        public string RegistrationNumber2 { get; set; }

        public string Name { get; set; }

        public string Building { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string County { get; set; }

        public string PostCode { get; set; }

        public Guid CountryId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }
    }
}