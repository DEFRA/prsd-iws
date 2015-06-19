namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Carrier
{
    using System;
    using Requests.Carriers;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class AddCarrierViewModel
    {
        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessViewModel Business { get; set; }

        public AddCarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }

        public AddCarrierToNotification ToRequest()
        {
            return new AddCarrierToNotification
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = (BusinessData)Business,
                Contact = Contact
            };
        }
    }
}