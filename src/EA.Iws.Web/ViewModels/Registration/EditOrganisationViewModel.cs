namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Registration;
    using Core.Shared;
    using Prsd.Core.Validation;

    public class EditOrganisationViewModel
    {
        public IEnumerable<CountryData> Countries { get; set; }

        public Guid OrganisationId { get; set; }

        public string Name { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Town")]
        public string TownOrCity { get; set; }

        [Display(Name = "County")]
        public string Region { get; set; }

        [RequiredIfPropertiesEqual("CountryId", "DefaultCountryId", "The Postcode field is required")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }

        public Guid DefaultCountryId { get; set; }

        public EditOrganisationViewModel()
        {
        }

        public EditOrganisationViewModel(OrganisationRegistrationData orgData)
        {
            OrganisationId = orgData.OrganisationId;
            Address1 = orgData.Address.StreetOrSuburb;
            Address2 = orgData.Address.Address2;
            TownOrCity = orgData.Address.TownOrCity;
            Postcode = orgData.Address.PostalCode;
            Region = orgData.Address.Region;
            Name = orgData.Name;
        }

        public OrganisationRegistrationData ToRequest()
        {
            return new OrganisationRegistrationData
            {
                OrganisationId = OrganisationId,
                Name = Name,
                Address = new AddressData
                {
                    StreetOrSuburb = Address1,
                    Address2 = Address2,
                    CountryId = CountryId,
                    PostalCode = Postcode,
                    Region = Region,
                    TownOrCity = TownOrCity
                }
            };
        }
    }
}