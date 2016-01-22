namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Facility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Shared;

    public class FacilityViewModel
    {
        public Guid FacilityId { get; set; }

        public AddressViewModel Address { get; set; }

        [Display(Name = "BusinessName", ResourceType = typeof(FacilityViewModelResources))]
        public string BusinessName { get; set; }

        [Display(Name = "Type", ResourceType = typeof(FacilityViewModelResources))]
        public BusinessType? Type { get; set; }

        [Display(Name = "RegistrationNumber", ResourceType = typeof(FacilityViewModelResources))]
        public string RegistrationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public string NotificationTypeString
        {
            get { return NotificationType.ToString().ToLower(); }
        }

        public bool IsActualSite { get; set; }

        public ContactViewModel Contact { get; set; }

        public FacilityViewModel()
        {
            FacilityId = Guid.NewGuid();
            Address = new AddressViewModel();
            Contact = new ContactViewModel();
        }

        public FacilityViewModel(Facility facility)
        {
            FacilityId = facility.Id;
            Address = new AddressViewModel(facility.Address);
            BusinessName = facility.BusinessName;
            Contact = new ContactViewModel(facility.Contact);
            RegistrationNumber = facility.RegistrationNumber;
            Type = facility.Type;
            IsActualSite = facility.IsActualSite;
        }

        public void DefaultUkIfUnselected(IEnumerable<CountryData> countries)
        {
            if (Address != null && !Address.CountryId.HasValue)
            {
                Address.CountryId =
                    countries.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id;
            }
        }
    }
}