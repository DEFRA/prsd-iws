namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Importer
{
    using System;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class ImporterViewModel
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public ImporterViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }
    }
}