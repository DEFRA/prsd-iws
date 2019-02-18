namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.CustomsOffice
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Prsd.Core.Validation;
    using Views.CustomsOffice;
    using Views.EntryCustomsOffice;

    public class CustomsOfficeViewModel
    {
        public CustomsOfficeViewModel()
        {
            this.CustomsOfficeRequired = null;
        }

        [RequiredIf("CustomsOfficeRequired", true, ErrorMessageResourceType = typeof(CustomsOfficeResources), ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Name", ResourceType = typeof(CustomsOfficeResources))]
        [StringLength(1024)]
        public string Name { get; set; }

        [RequiredIf("CustomsOfficeRequired", true, ErrorMessageResourceType = typeof(CustomsOfficeResources), ErrorMessageResourceName = "AddressRequired")]
        [Display(Name = "Address", ResourceType = typeof(CustomsOfficeResources))]
        [StringLength(4000)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public SelectList Countries { get; set; }

        [RequiredIf("CustomsOfficeRequired", true, ErrorMessageResourceType = typeof(CustomsOfficeResources), ErrorMessageResourceName = "CountryRequired")]
        [Display(Name = "Country", ResourceType = typeof(CustomsOfficeResources))]
        public Guid? SelectedCountry { get; set; }

        public int Steps { get; set; }

        [Required(ErrorMessageResourceType = typeof(CustomsOfficeResources), ErrorMessageResourceName = "TransportRouteRequired")]
        public bool? CustomsOfficeRequired { get; set; }
    }
}