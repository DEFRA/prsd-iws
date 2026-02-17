namespace EA.Iws.Core.Notification.Overview
{
    using EA.Iws.Core.NotificationAssessment;
    using Notification;
    using Shared;
    using System;

    public class NotificationOverview
    {
        public NotificationApplicationCompletionProgress Progress { get; set; }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public OrganisationsInvolved OrganisationsInvolved { get; set; }

        public InterimStatus InterimStatus { get; set; }

        public RecoveryOperation RecoveryOperation { get; set; }

        public Transportation Transportation { get; set; }

        public Journey Journey { get; set; }

        public WasteClassificationOverview WasteClassificationOverview { get; set; }

        public WasteRecoveryOverview WasteRecovery { get; set; }

        public WasteDisposalOverview WasteDisposal { get; set; }

        public WasteCodesOverviewInfo WasteCodesOverview { get; set; }

        public ShipmentOverview ShipmentOverview { get; set; }

        public SubmitSummaryData SubmitSummaryData { get; set; }

        public decimal NotificationCharge { get; set; }

        public bool CanEditNotification { get; set; }
    }
}