namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using Requests.Exporters;
    using Requests.Producers;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class AddProducerViewModel
    {
        public AddProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }

        public AddProducerViewModel(ExporterData exporter)
        {
            NotificationId = exporter.NotificationId;
            Address = exporter.Address;
            Contact = exporter.Contact;
            Business = new BusinessViewModel(exporter.Business);
        }

        public AddProducerToNotification ToRequest()
        {
            return new AddProducerToNotification
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = (BusinessData)Business,
                Contact = Contact
            };
        }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }
    }
}