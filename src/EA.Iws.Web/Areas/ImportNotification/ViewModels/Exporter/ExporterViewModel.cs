namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Exporter
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;
    using Shared;

    public class ExporterViewModel
    {
        [Display(Name = "BusinessName", ResourceType = typeof(ExporterViewModelResources))]
        public string BusinessName { get; set; }

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
            BusinessName = exporter.BusinessName;

            Address = new AddressViewModel(exporter.Address);

            Contact = new ContactViewModel(exporter.Contact);

            IsAddedToAddressBook = exporter.IsAddedToAddressBook;
        }
    }
}