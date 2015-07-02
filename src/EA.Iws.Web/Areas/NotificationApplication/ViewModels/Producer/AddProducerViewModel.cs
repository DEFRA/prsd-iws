namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using Core.Exporters;
    using Core.Shared;
    using Requests.Producers;

    public class AddProducerViewModel
    {
        public AddProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new ProducerBusinessTypeViewModel();
        }

        public AddProducerViewModel(ExporterData exporter)
        {
            NotificationId = exporter.NotificationId;
            Address = exporter.Address;
            Contact = exporter.Contact;
            Business = new ProducerBusinessTypeViewModel(exporter.Business);
        }

        public AddProducerToNotification ToRequest()
        {
            return new AddProducerToNotification
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact
            };
        }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public ProducerBusinessTypeViewModel Business { get; set; }
    }
}