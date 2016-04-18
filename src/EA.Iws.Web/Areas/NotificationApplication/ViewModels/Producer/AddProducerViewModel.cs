namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
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

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public ProducerBusinessTypeViewModel Business { get; set; }

        public bool IsAddedToAddressBook { get; set; }

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
    }
}