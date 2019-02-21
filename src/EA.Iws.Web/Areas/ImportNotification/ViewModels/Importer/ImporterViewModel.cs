namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Importer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using Shared;

    public class ImporterViewModel
    {
        public AddressViewModel Address { get; set; }

        public BusinessViewModel Business { get; set; }

        [Display(Name = "Type", ResourceType = typeof(ImporterViewModelResources))]
        public BusinessType? BusinessType { get; set; }

        public ContactViewModel Contact { get; set; }
        
        public ImporterViewModel()
        {
        }

        public ImporterViewModel(Importer importer)
        {
            Address = new AddressViewModel(importer.Address);
            Business = new BusinessViewModel(importer.BusinessName, importer.RegistrationNumber);
            Contact = new ContactViewModel(importer.Contact);
            BusinessType = importer.Type;
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