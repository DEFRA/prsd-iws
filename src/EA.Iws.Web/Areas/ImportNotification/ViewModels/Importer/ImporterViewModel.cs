namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Importer
{
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using EA.Iws.Web.Areas.ImportNotification.Views.Importer;
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ImporterViewModel
    {
        public AddressViewModel Address { get; set; }

        public BusinessViewModel Business { get; set; }

        [Required(ErrorMessageResourceName = "OrganisationTypeRequired", ErrorMessageResourceType = typeof(IndexResources))]
        [Display(Name = "OrganisationType", ResourceType = typeof(IndexResources))]
        public BusinessType? BusinessType { get; set; }

        public ContactViewModel Contact { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public ImporterViewModel()
        {
        }

        public ImporterViewModel(Importer importer)
        {
            Address = new AddressViewModel(importer.Address, AddressTypeEnum.Importer);
            Business = new BusinessViewModel(importer.BusinessName, importer.RegistrationNumber);
            Contact = new ContactViewModel(importer.Contact, AddressTypeEnum.Importer);
            BusinessType = importer.Type;
            IsAddedToAddressBook = importer.IsAddedToAddressBook;
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