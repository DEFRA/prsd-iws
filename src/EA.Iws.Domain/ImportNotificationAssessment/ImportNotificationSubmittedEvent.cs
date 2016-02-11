namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using Prsd.Core.Domain;

    public class ImportNotificationSubmittedEvent : IEvent
    {
        public ImportNotificationAssessment Assessment { get; private set; }

        public ImportNotificationSubmittedEvent(ImportNotificationAssessment importNotificationAssessment)
        {
            this.Assessment = importNotificationAssessment;
        }
    }
}