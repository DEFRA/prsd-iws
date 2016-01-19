namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.Notification.Overview;
    using Core.Shared;

    public class NotificationOverviewViewModel
    {
        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

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

        public NotificationOverviewViewModel(NotificationOverview overviewData)
        {
            NotificationType = overviewData.NotificationType;
            NotificationId = overviewData.NotificationId;
            NotificationNumber = overviewData.NotificationNumber;
            OrganisationsInvolvedViewModel = new OrganisationsInvolvedViewModel(overviewData.OrganisationsInvolved, overviewData.Progress);
            RecoveryOperationViewModel = new RecoveryOperationViewModel(overviewData.RecoveryOperation, overviewData.Progress);
            TransportationViewModel = new TransportationViewModel(overviewData.Transportation, overviewData.Progress);
            JourneyViewModel = new JourneyViewModel(overviewData.Journey, overviewData.Progress);
            ClassifyYourWasteViewModel = new ClassifyYourWasteViewModel(overviewData.WasteClassificationOverview, overviewData.Progress);
            WasteRecoveryViewModel = new WasteRecoveryViewModel(overviewData.NotificationId, overviewData.WasteRecovery, overviewData.WasteDisposal, overviewData.Progress);
            AmountsAndDatesViewModel = new AmountsAndDatesViewModel(overviewData.ShipmentOverview, overviewData.Progress);
            SubmitSideBarViewModel = new SubmitSideBarViewModel(overviewData.SubmitSummaryData, overviewData.NotificationCharge, overviewData.Progress);
            WasteCodeOverviewViewModel = new WasteCodeOverviewViewModel(overviewData.WasteCodesOverview, overviewData.Progress);
            NotificationCharge = overviewData.NotificationCharge;
            CanEditNotification = overviewData.CanEditNotification;
        }
    }
}