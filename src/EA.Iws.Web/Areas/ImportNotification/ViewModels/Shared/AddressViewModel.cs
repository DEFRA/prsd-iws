namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
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
    }
}