namespace EA.Iws.Requests.Notification
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Shared;

    public class ExporterData
    {
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        public string Type { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Additional Registration Number")]
        public string AdditionalRegistrationNumber { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public Guid NotificationId { get; set; }

        public Guid ExporterId { get; set; }
    }
}
