namespace EA.Iws.Web.ViewModels.Carrier
{
    using System;
    using Requests.Shared;
    using Shared;

    public class CarrierViewModel
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public CarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }
    }
}