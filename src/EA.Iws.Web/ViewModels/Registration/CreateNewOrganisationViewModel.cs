namespace EA.Iws.Web.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CreateNewOrganisationViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganisationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Companies House Number")]
        public string CompaniesHouseReference { get; set; }

        [Required]
        [Display(Name = "Town or city")]
        public string TownOrCity { get; set; }

        public string CountyOrProvince { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required]
        public string Postcode { get; set; }
        public string Country { get; set; }

        [Required]
        [Display(Name = "Organisation Type")]
        public string EntityType { get; set; }
    }
}