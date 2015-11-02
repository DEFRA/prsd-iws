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

        public ExporterViewModel()
        {
            Address = new AddressViewModel();

            Contact = new ContactViewModel();
        }

        public ExporterViewModel(Exporter exporter)
        {
            BusinessName = exporter.BusinessName;

            Address = new AddressViewModel
            {
                AddressLine1 = exporter.AddressLine1,
                AddressLine2 = exporter.AddressLine2,
                CountryId = exporter.CountryId,
                PostalCode = exporter.PostalCode,
                TownOrCity = exporter.TownOrCity
            };

            Contact = new ContactViewModel
            {
                Email = exporter.Email,
                Name = exporter.ContactName,
                Telephone = exporter.Telephone
            };
        }
    }
}