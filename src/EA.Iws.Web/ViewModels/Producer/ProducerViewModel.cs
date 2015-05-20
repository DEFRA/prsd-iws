namespace EA.Iws.Web.ViewModels.Producer
{
    using System;
    using Requests.Shared;
    using Shared;

    public class ProducerViewModel
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public bool IsSiteOfExport { get; set; }

        public ProducerViewModel()
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