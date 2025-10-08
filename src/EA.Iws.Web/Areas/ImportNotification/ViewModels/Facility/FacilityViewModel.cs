namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Facility
{
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using EA.Iws.Web.Areas.ImportNotification.Views.Facility;
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class FacilityViewModel
    {
        public Guid FacilityId { get; set; }

        public AddressViewModel Address { get; set; }

        public BusinessViewModel Business { get; set; }

        [Required(ErrorMessageResourceName = "OrganisationTypeRequired", ErrorMessageResourceType = typeof(AddResources))]
        [Display(Name = "OrganisationType", ResourceType = typeof(AddResources))]
        public BusinessType? BusinessType { get; set; }

        public NotificationType NotificationType { get; set; }

        public string NotificationTypeString
        {
            get { return NotificationType.ToString().ToLower(); }
        }

        public bool IsActualSite { get; set; }

        public ContactViewModel Contact { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public FacilityViewModel()
        {
            FacilityId = Guid.NewGuid();
            Address = new AddressViewModel(AddressTypeEnum.Facility);
            Contact = new ContactViewModel(AddressTypeEnum.Facility);
            Business = new BusinessViewModel();
        }

        public FacilityViewModel(Facility facility)
        {
            FacilityId = facility.Id;
            Address = new AddressViewModel(facility.Address, AddressTypeEnum.Facility);
            Business = new BusinessViewModel(facility.BusinessName, facility.RegistrationNumber, facility.AdditionalRegistrationNumber);
            Contact = new ContactViewModel(facility.Contact, AddressTypeEnum.Facility);
            BusinessType = facility.Type;
            IsActualSite = facility.IsActualSite;
            IsAddedToAddressBook = facility.IsAddedToAddressBook;
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