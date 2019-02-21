namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Exporter
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;
    using Shared;

    public class ExporterViewModel
    {
        public BusinessViewModel Business { get; set; }

        public AddressViewModel Address { get; set; }

        public ContactViewModel Contact { get; set; }

        public ExporterViewModel()
        {
            Address = new AddressViewModel();

            Contact = new ContactViewModel();
        }

        public ExporterViewModel(Exporter exporter)
        {
            Business = new BusinessViewModel(exporter.BusinessName);

            Address = new AddressViewModel(exporter.Address);

            Contact = new ContactViewModel(exporter.Contact);
        }
    }
}