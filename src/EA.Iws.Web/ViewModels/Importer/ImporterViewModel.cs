namespace EA.Iws.Web.ViewModels.Importer
{
    using System;
    using Requests.Shared;
    using Shared;

    public class ImporterViewModel
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public ImporterViewModel()
        {
            if (Address == null)
            {
                Address = new AddressData();
            }

            if (Contact == null)
            {
                Contact = new ContactData();
            }

            if (Business == null)
            {
                Business = new BusinessViewModel();
            }
        }
    }
}