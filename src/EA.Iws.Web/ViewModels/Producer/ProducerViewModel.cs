namespace EA.Iws.Web.ViewModels.Producer
{
    using System;
    using Requests.Shared;
    using Shared;

    public class ProducerViewModel
    {
        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public ProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }
    }
}