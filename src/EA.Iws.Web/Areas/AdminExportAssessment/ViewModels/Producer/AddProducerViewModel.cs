namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Producer
{
    using Core.Shared;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using NotificationApplication.ViewModels.Producer;
    using System;

    public class AddProducerViewModel
    {
        public AddProducerViewModel()
        {
            Address = new AddressData();

            Contact = new ContactData();

            Business = new ProducerBusinessTypeViewModel();
        }

        public AddProducerViewModel(Guid notificationId, UKCompetentAuthority competentAuthority, NotificationStatus notificationStatus)
        {
            NotificationId = notificationId;
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = notificationId
            };
            CompetentAuthority = competentAuthority;
            NotificationStatus = notificationStatus;
            ShowAdditionalCharge = ((competentAuthority == UKCompetentAuthority.England ||
                                competentAuthority == UKCompetentAuthority.Scotland) &&
                                ((notificationStatus == NotificationStatus.Consented) ||
                                (notificationStatus == NotificationStatus.ConsentedUnlock) ||
                                (notificationStatus == NotificationStatus.Transmitted) ||
                                (notificationStatus == NotificationStatus.DecisionRequiredBy) ||
                                (notificationStatus == NotificationStatus.Reassessment))) ? true : false;

            if (ShowAdditionalCharge == false)
            {
                AdditionalCharge.IsAdditionalChargesRequired = false;
            }

            Address = new AddressData();

            Contact = new ContactData();

            Business = new ProducerBusinessTypeViewModel();
        }

        public Guid NotificationId { get; set; }

        public AddressData Address { get; set; }

        public ContactData Contact { get; set; }

        public ProducerBusinessTypeViewModel Business { get; set; }

        public bool IsAddedToAddressBook { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }

        public NotificationStatus NotificationStatus { get; set; }
    }
}