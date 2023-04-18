namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Exporter
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;
    using EA.Iws.Core.Shared;
    using Shared;

    public class ExporterViewModel
    {
        public BusinessViewModel Business { get; set; }

        [Display(Name = "Organisation type")]
        public BusinessType BusinessType { get; set; }

        public AddressViewModel Address { get; set; }

        public ContactViewModel Contact { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public ExporterViewModel()
        {
            Address = new AddressViewModel();

            Contact = new ContactViewModel();
        }

        public ExporterViewModel(Exporter exporter)
        {
            Business = new BusinessViewModel(exporter.BusinessName, exporter.RegistrationNumber);

            BusinessType = exporter.Type;

            Address = new AddressViewModel(exporter.Address, AddressTypeEnum.Exporter);

            Contact = new ContactViewModel(exporter.Contact, AddressTypeEnum.Exporter);

            IsAddedToAddressBook = exporter.IsAddedToAddressBook;
        }
    }
}