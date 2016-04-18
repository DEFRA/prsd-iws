namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using Core.Producers;
    using Core.Shared;
    using Requests.Producers;

    public class EditProducerViewModel : AddProducerViewModel
    {
        public Guid ProducerId { get; set; }

        public EditProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new ProducerBusinessTypeViewModel();
        }

        public EditProducerViewModel(ProducerData producer)
        {
            NotificationId = producer.NotificationId;
            ProducerId = producer.Id;
            Address = producer.Address;
            Contact = producer.Contact;
            Business = new ProducerBusinessTypeViewModel(producer.Business);   
        }

        public new UpdateProducerForNotification ToRequest()
        {
            return new UpdateProducerForNotification
            {
                ProducerId = ProducerId,
                NotificationId = NotificationId,
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact
            };
        }
    }
}