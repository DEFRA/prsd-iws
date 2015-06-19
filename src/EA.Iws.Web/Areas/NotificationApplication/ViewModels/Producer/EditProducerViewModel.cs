namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using Requests.Producers;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class EditProducerViewModel : AddProducerViewModel
    {
        public Guid ProducerId { get; set; }

        public EditProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }

        public EditProducerViewModel(ProducerData producer)
        {
            NotificationId = producer.NotificationId;
            ProducerId = producer.Id;
            Address = producer.Address;
            Contact = producer.Contact;
            Business = (BusinessViewModel)producer.Business;   
        }

        public new UpdateProducerForNotification ToRequest()
        {
            return new UpdateProducerForNotification
            {
                ProducerId = ProducerId,
                NotificationId = NotificationId,
                Address = Address,
                Business = (BusinessData)Business,
                Contact = Contact
            };
        }
    }
}