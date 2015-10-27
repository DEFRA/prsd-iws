namespace EA.Iws.Core.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class AddressData : IValidatableObject
    {
        private const string DefaultCountryName = "United Kingdom";

        [Required(ErrorMessageResourceType = typeof(AddressDataResources), ErrorMessageResourceName = "Address1Required")]
        [Display(Name = "Address1", ResourceType = typeof(AddressDataResources))]
        public string StreetOrSuburb { get; set; }

        [Display(Name = "Address2", ResourceType = typeof(AddressDataResources))]
        public string Address2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(AddressDataResources), ErrorMessageResourceName = "CityRequired")]
        [Display(Name = "City", ResourceType = typeof(AddressDataResources))]
        public string TownOrCity { get; set; }

        [Display(Name = "Postcode", ResourceType = typeof(AddressDataResources))]
        [RequiredIfPropertiesEqual("CountryId", "DefaultCountryId", ErrorMessageResourceName = "PostcodeRequired", ErrorMessageResourceType = typeof(AddressDataResources))]
        public string PostalCode { get; set; }

        [Display(Name = "County", ResourceType = typeof(AddressDataResources))]
        public string Region { get; set; }

        [Required(ErrorMessageResourceType = typeof(AddressDataResources), ErrorMessageResourceName = "CountryRequired")]
        [Display(Name = "Country", ResourceType = typeof(AddressDataResources))]
        public Guid? CountryId { get; set; }

        public Guid DefaultCountryId { get; set; }

        [Display(Name = "Country", ResourceType = typeof(AddressDataResources))]
        public string CountryName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CountryId == Guid.Empty)
            {
                yield return new ValidationResult(AddressDataResources.CountrySelect, new[] { "CountryId" });
            }
        }
    }
}
