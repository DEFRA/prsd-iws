namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Carrier
{
    using System;
    using Core.Carriers;
    using Core.Shared;
    using Requests.Carriers;
    using Requests.Shared;
    using Web.ViewModels.Shared;

    public class EditCarrierViewModel : AddCarrierViewModel
    {
        public Guid CarrierId { get; set; }

        public EditCarrierViewModel(CarrierData carrier)
        {
            Address = carrier.Address;
            Business = (BusinessViewModel)carrier.Business;
            Contact = carrier.Contact;
            NotificationId = carrier.NotificationId;
            CarrierId = carrier.Id;
        }

        public EditCarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessViewModel();
        }

        public new UpdateCarrierForNotification ToRequest()
        {
            return new UpdateCarrierForNotification
            {
                Address = Address,
                Business = (BusinessData)Business,
                Contact = Contact,
                NotificationId = NotificationId,
                CarrierId = CarrierId
            };
        }
    }
}