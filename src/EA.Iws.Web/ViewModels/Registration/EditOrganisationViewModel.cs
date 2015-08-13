namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.Registration;
    using Core.Shared;
    using Prsd.Core.Validation;

    public class EditOrganisationViewModel
    {
        private const string DefaultCountryName = "United Kingdom";

        public IEnumerable<CountryData> Countries { get; set; }

        public Guid OrganisationId { get; set; }

        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address line 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address line 2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Town")]
        public string TownOrCity { get; set; }

        [RequiredIfPropertiesEqual("CountryId", "DefaultCountryId", "The Postcode field is required")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }

        public Guid DefaultCountryId { get; set; }

        [Required]
        [Display(Name = "Organisation type")]
        public BusinessType BusinessType { get; set; }

        [RequiredIf("BusinessType", BusinessType.Other, "Description is required")]
        [Display(Name = "Organisation type")]
        public string OtherDescription { get; set; }

        public CountryData DefaultCountry
        {
            get
            {
                if (Countries == null || !Countries.Any())
                {
                    return null;
                }

                var country = Countries.SingleOrDefault(c => c.Name.Equals(DefaultCountryName));

                if (this.CountryId == Guid.Empty)
                {
                    if (country != null)
                    {
                        this.CountryId = country.Id;
                        DefaultCountryId = country.Id;
                    }
                }

                return country ?? Countries.First();
            }
        }

        public EditOrganisationViewModel()
        {
        }

        public EditOrganisationViewModel(OrganisationRegistrationData orgData)
        {
            OrganisationId = orgData.OrganisationId;
            Name = orgData.Name;
            Address1 = orgData.Address1;
            Address2 = orgData.Address2;
            TownOrCity = orgData.TownOrCity;
            Postcode = orgData.Postcode;
            CountryId = orgData.CountryId;
            BusinessType = orgData.BusinessType;
            OtherDescription = orgData.OtherDescription;
        }

        public OrganisationRegistrationData ToRequest()
        {
            return new OrganisationRegistrationData
            {
                OrganisationId = OrganisationId,
                Name = Name,
                Address1 = Address1,
                Address2 = Address2,
                TownOrCity = TownOrCity,
                Postcode = Postcode,
                CountryId = CountryId,
                BusinessType = BusinessType,
                OtherDescription = OtherDescription
            };
        }
    }
}