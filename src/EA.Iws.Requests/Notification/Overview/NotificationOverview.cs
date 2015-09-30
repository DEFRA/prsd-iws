namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using Core.Notification;

    public class NotificationOverview
    {
        public Guid NotificationId { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public string CompetentAuthorityName { get; set; }

        public OrganisationsInvolved OrganisationsInvolvedInfo { get; set; }

        public RecoveryOperation RecoveryOperationInfo { get; set; }

        public Transportation TransportationInfo { get; set; }

        public Journey JourneyInfo { get; set; }

        public ClassifyYourWaste ClassifyYourWasteInfo { get; set; }

        public WasteRecovery WasteRecoveryInfo { get; set; }

        public WasteCodesOverviewInfo WasteCodesOverviewInfo { get; set; }

        public AmountsAndDates AmountsAndDatesInfo { get; set; }

        public SubmitSummaryData SubmitSummaryData { get; set; }

        public int NotificationCharge { get; set; }

        public bool CanEditNotification { get; set; }
    }
}