namespace EA.Iws.Requests.Notification
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Shared;

    public class ProducerData
    {
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        public bool IsSiteOfExport { get; set; }

        public string Type { get; set; }

        [Display(Name = "Registration Number")]
        public string CompaniesHouseNumber { get; set; }

        public string RegistrationNumber1 { get; set; }

        public string AdditionalRegistrationNumber { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        public Guid ProducerId { get; set; }
    }
}