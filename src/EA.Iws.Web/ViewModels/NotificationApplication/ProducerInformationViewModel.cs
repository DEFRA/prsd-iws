namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Web;
    using Shared;

    public class ProducerInformationViewModel
    {
        public Guid NotificationId { get; set; }

        public BusinessNameAndTypeViewModel BusinessNameAndTypeViewModel { get; set; }

        public AddressViewModel AddressDetails { get; set; }

        public ContactPersonViewModel ContactDetails { get; set; }

        [Required]
        [Display(Name = "This producer is the Site of Export")]
        public bool IsSiteOfExport { get; set; }
    }
}