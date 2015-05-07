namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class AddressViewModel
    {
        [Required]
        [Display(Name = "Building name or number")]
        public string Building { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Town or city")]
        public string TownOrCity { get; set; }

        [Display(Name = "County")]
        public string County { get; set; }

        [Required]
        public string Postcode { get; set; }
    }
}