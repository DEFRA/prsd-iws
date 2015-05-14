namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure;

    public class BusinessNameAndTypeViewModel
    {
        [Required]
        [Display(Name = "Organisation Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation Type")]
        public string EntityType { get; set; }

        [RequiredIf("EntityType", "Limited Company", "Companies house number is required")]
        [Display(Name = "Companies House Number")]
        public string CompaniesHouseReference { get; set; }

        [RequiredIf("EntityType", "Sole Trader", "Sole Trader registration number is required")]
        [Display(Name = "Registration Number")]
        public string SoleTraderRegistrationNumber { get; set; }

        [RequiredIf("EntityType", "Partnership", "Partnership registration number is required")]
        [Display(Name = "Registration Number")]
        public string PartnershipRegistrationNumber { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber2 { get; set; }
    }
}