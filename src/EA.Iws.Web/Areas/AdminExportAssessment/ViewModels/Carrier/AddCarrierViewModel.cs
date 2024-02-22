namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Carrier
{
    using Core.Shared;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using Requests.NotificationAssessment;
    using System;
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

        public NotificationStatus NotificationStatus { get; set; }

        public AddCarrierViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new BusinessTypeViewModel();
        }

        public AddCarrierViewModel(Guid notificationId, UKCompetentAuthority competentAuthority, NotificationStatus notificationStatus)
        {
            NotificationId = notificationId;
            AdditionalCharge = new AdditionalChargeData() { NotificationId = notificationId };
            CompetentAuthority = competentAuthority;
            NotificationStatus = notificationStatus;
            ShowAdditionalCharge = ((competentAuthority == UKCompetentAuthority.England ||
                                competentAuthority == UKCompetentAuthority.Scotland) &&
                                ((notificationStatus == NotificationStatus.Consented) ||
                                (notificationStatus == NotificationStatus.ConsentedUnlock) ||
                                (notificationStatus == NotificationStatus.Transmitted) ||
                                (notificationStatus == NotificationStatus.DecisionRequiredBy) ||
                                (notificationStatus == NotificationStatus.Reassessment))) ? true : false;

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