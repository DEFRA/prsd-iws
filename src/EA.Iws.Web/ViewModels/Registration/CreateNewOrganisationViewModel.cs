namespace EA.Iws.Web.ViewModels.Registration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web.Mvc;
    using Api.Client.Entities;
    using Infrastructure;

    public class CreateNewOrganisationViewModel
    {
        private const string DefaultCountryName = "United Kingdom";

        public IEnumerable<CountryData> Countries { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganisationId { get; set; }

        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [RequiredIf("EntityType", "Limited Company", "Companies house number is required")]
        [Display(Name = "Companies house number")]
        public string CompaniesHouseReference { get; set; }

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

        [Required]
        public string Postcode { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [Display(Name = "Organisation Type")]
        public string EntityType { get; set; }

        public CountryData DefaultCountry
        {
            get
            {
                if (Countries == null || !Countries.Any())
                {
                    return null;
                }
                var country = Countries.SingleOrDefault(c => c.Name.Equals(DefaultCountryName));
                return country ?? Countries.First();
            }
        }
    }
}