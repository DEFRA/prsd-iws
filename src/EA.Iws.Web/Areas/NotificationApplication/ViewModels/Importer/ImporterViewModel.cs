namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Importer
{
    using System;
    using Core.Importer;
    using Core.Shared;
    using Requests.Importer;
    using Web.ViewModels.Shared;

    public class ImporterViewModel
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessTypeViewModel Business { get; set; }

        public ImporterViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public ImporterViewModel(ImporterData importer)
        {
            NotificationId = importer.NotificationId;
            Address = importer.Address;
            Contact = importer.Contact;
            Business = new BusinessTypeViewModel(importer.Business);
        }

        public SetImporterForNotification ToRequest()
        {
            return new SetImporterForNotification
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact
            };
        }
    }
}