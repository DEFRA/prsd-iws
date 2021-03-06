﻿namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Registration;
    using Core.Shared;
    using Prsd.Core.Validation;
    using Views.Registration;

    public class EditOrganisationViewModel
    {
        public IEnumerable<CountryData> Countries { get; set; }

        public Guid OrganisationId { get; set; }

        public string Name { get; set; }

        [Required]
        [Display(Name = "AddressLine1", ResourceType = typeof(EditOrganisationDetailsResources))]
        public string Address1 { get; set; }

        [Display(Name = "AddressLine2", ResourceType = typeof(EditOrganisationDetailsResources))]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "Town", ResourceType = typeof(EditOrganisationDetailsResources))]
        public string TownOrCity { get; set; }

        [Display(Name = "County", ResourceType = typeof(EditOrganisationDetailsResources))]
        public string Region { get; set; }

        [RequiredIfPropertiesEqual("CountryId", "DefaultCountryId", ErrorMessageResourceName = "PostcodeRequired", ErrorMessageResourceType = typeof(EditOrganisationDetailsResources))]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Country", ResourceType = typeof(EditOrganisationDetailsResources))]
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