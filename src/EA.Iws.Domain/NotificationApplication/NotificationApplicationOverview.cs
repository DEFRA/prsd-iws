namespace EA.Iws.Domain.NotificationApplication
{
    using Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain.ImportNotification;
    using NotificationAssessment;
    using WasteRecovery;

    public class NotificationApplicationOverview
    {
        public NotificationApplication Notification { get; private set; }

        public NotificationAssessment NotificationAssessment { get; private set; }

        public FacilityCollection Facilities { get; private set; }

        public WasteRecovery.WasteRecovery WasteRecovery { get; private set; }

        public WasteDisposal WasteDisposal { get; private set; }

        public Exporter.Exporter Exporter { get; private set; }

        public Importer.Importer Importer { get; private set; }

        public decimal Charge { get; private set; }

        public NotificationApplicationCompletionProgress Progress { get; private set; }

        public static NotificationApplicationOverview Load(NotificationApplication notification,
            NotificationAssessment assessment,
            FacilityCollection facilities,
            WasteRecovery.WasteRecovery wasteRecovery,
            WasteDisposal wasteDisposal,
            Exporter.Exporter exporter,
            Importer.Importer importer,
            decimal charge,
            NotificationApplicationCompletionProgress progress)
        {
            return new NotificationApplicationOverview
            {
                Exporter = exporter,
                Importer = importer,
                Notification = notification,
                NotificationAssessment = assessment,
                Facilities = facilities,
                WasteRecovery = wasteRecovery,
                WasteDisposal = wasteDisposal,
                Charge = charge,
                Progress = progress
            };
        }
    }
}