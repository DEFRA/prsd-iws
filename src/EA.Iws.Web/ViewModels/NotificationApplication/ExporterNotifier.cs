namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Infrastructure;
    using Shared;

    public class ExporterNotifier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExporterId { get; set; }

        public Guid NotificationId { get; set; }

        [Required]
        [Display(Name = "Organisation Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation Type")]
        public string EntityType { get; set; }

        [RequiredIf("EntityType", "Limited Company", "Companies house number is required")]
        [Display(Name = "Company House Number")]
        public string CompaniesHouseReference { get; set; }

        [RequiredIf("EntityType", "Sole Trader", "Sole Trader registration number is required")]
        [Display(Name = "Registration Number")]
        public string SoleTraderRegistrationNumber { get; set; }

        [RequiredIf("EntityType", "Partnership", "Partnership registration number is required")]
        [Display(Name = "Registration Number")]
        public string PartnershipRegistrationNumber { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber2 { get; set; }

        public AddressViewModel AddressDetails { get; set; }

        public ContactPersonViewModel ContactDetails { get; set; }
    }
}