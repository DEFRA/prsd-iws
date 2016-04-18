namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Carrier
{
    using System;
    using Core.Carriers;
    using Core.Shared;
    using Requests.Carriers;
    using Web.ViewModels.Shared;

    public class EditCarrierViewModel : AddCarrierViewModel
    {
        public Guid CarrierId { get; set; }

        public EditCarrierViewModel(CarrierData carrier)
        {
            Address = carrier.Address;
            Business = new BusinessTypeViewModel(carrier.Business);
            Contact = carrier.Contact;
            NotificationId = carrier.NotificationId;
            CarrierId = carrier.Id;
        }

        public EditCarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public new UpdateCarrierForNotification ToRequest()
        {
            return new UpdateCarrierForNotification
            {
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact,
                NotificationId = NotificationId,
                CarrierId = CarrierId
            };
        }
    }
}