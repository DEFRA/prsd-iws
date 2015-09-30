namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Requests.Notification.Overview;

    public class NotificationOverviewViewModel
    {
        public string NotificationNumber { get; set; }

        public Guid NotificationId { get; set; }

        public OrganisationsInvolvedViewModel OrganisationsInvolvedViewModel { get; set; }

        public RecoveryOperationViewModel RecoveryOperationViewModel { get; set; }

        public TransportationViewModel TransportationViewModel { get; set; }

        public JourneyViewModel JourneyViewModel { get; set; }

        public ClassifyYourWasteViewModel ClassifyYourWasteViewModel { get; set; }

        public WasteCodeOverviewViewModel WasteCodeOverviewViewModel { get; set; }

        public WasteRecoveryViewModel WasteRecoveryViewModel { get; set; }

        public AmountsAndDatesViewModel AmountsAndDatesViewModel { get; set; }

        public SubmitSideBarViewModel SubmitSideBarViewModel { get; set; }

        public int NotificationCharge { get; set; }

        public bool CanEditNotification { get; set; }

        public NotificationOverviewViewModel()
        {
        }

        public NotificationOverviewViewModel(NotificationOverview notificationInfo)
        {
            NotificationId = notificationInfo.NotificationId;
            NotificationNumber = notificationInfo.NotificationNumber;
            OrganisationsInvolvedViewModel = new OrganisationsInvolvedViewModel(notificationInfo.OrganisationsInvolvedInfo);
            RecoveryOperationViewModel = new RecoveryOperationViewModel(notificationInfo.RecoveryOperationInfo);
            TransportationViewModel = new TransportationViewModel(notificationInfo.TransportationInfo);
            JourneyViewModel = new JourneyViewModel(notificationInfo.JourneyInfo);
            ClassifyYourWasteViewModel = new ClassifyYourWasteViewModel(notificationInfo.ClassifyYourWasteInfo);
            WasteRecoveryViewModel = new WasteRecoveryViewModel(notificationInfo.WasteRecoveryInfo);
            AmountsAndDatesViewModel = new AmountsAndDatesViewModel(notificationInfo.AmountsAndDatesInfo);
            SubmitSideBarViewModel = new SubmitSideBarViewModel(notificationInfo.SubmitSummaryData, notificationInfo.NotificationCharge);
            WasteCodeOverviewViewModel = new WasteCodeOverviewViewModel(notificationInfo.WasteCodesOverviewInfo);
            NotificationCharge = notificationInfo.NotificationCharge;
            CanEditNotification = notificationInfo.CanEditNotification;
        }
    }
}