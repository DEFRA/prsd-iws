namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Carrier
{
    using System;
    using Core.Shared;
    using EA.Iws.Core.Notification;
    using Requests.NotificationAssessment;
    using Web.ViewModels.Shared;

    public class AddCarrierViewModel
    {
        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public BusinessTypeViewModel Business { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }

        public AddCarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public AddCarrier ToRequest()
        {
            return new AddCarrier
            {
                NotificationId = NotificationId,
                Address = Address,
                Business = Business.ToBusinessInfoData(),
                Contact = Contact
            };
        }
    }
}