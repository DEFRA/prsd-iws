namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Core.Shared;

    public class AddressViewModel
    {
        [Display(Name = "AddressLine1", ResourceType = typeof(AddressViewModelResources))]
        public string AddressLine1 { get; set; }

        [Display(Name = "AddressLine2", ResourceType = typeof(AddressViewModelResources))]
        public string AddressLine2 { get; set; }

        [Display(Name = "TownOrCity", ResourceType = typeof(AddressViewModelResources))]
        public string TownOrCity { get; set; }

        [Display(Name = "PostalCode", ResourceType = typeof(AddressViewModelResources))]
        public string PostalCode { get; set; }

        [Display(Name = "CountryId", ResourceType = typeof(AddressViewModelResources))]
        public Guid? CountryId { get; set; }

        public SelectList CountrySelectList
        {
            get
            {
                return new SelectList(Countries, "Id", "Name");
            }
        }

        public IList<CountryData> Countries { get; set; }

        public AddressViewModel()
        {
        }

        public AddressViewModel(Address address)
        {
            if (address != null)
            {
                AddressLine1 = address.AddressLine1;
                AddressLine2 = address.AddressLine2;
                TownOrCity = address.TownOrCity;
                PostalCode = address.PostalCode;
                CountryId = address.CountryId;
            }
        }

        public Address AsAddress()
        {
            return new Address
            {
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                TownOrCity = TownOrCity,
                PostalCode = PostalCode,
                CountryId = CountryId
            };
        }
    }
}