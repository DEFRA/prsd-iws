namespace EA.Iws.Requests.Shared
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class AddressData
    {
        private const string DefaultCountryName = "United Kingdom";

        [Required]
        [Display(Name = "Building name or number")]
        public string Building { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string StreetOrSuburb { get; set; }

        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Town or city")]
        public string TownOrCity { get; set; }

        [Display(Name = "Postcode")]
        [RequiredIfPropertiesEqual("CountryId", "DefaultCountryId", "The Postcode field is required")]
        public string PostalCode { get; set; }

        [Display(Name = "County")]
        public string Region { get; set; }

        [Required]
        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public Guid DefaultCountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }
    }
}
