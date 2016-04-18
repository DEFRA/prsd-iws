namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Domain;

    public class ImportNotificationStatusChangeEvent : IEvent
    {
        public ImportNotificationAssessment Assessment { get; private set; }

        public ImportNotificationStatus Source { get; private set; }

        public ImportNotificationStatus Destination { get; private set; }

        public ImportNotificationStatusChangeEvent(ImportNotificationAssessment assessment, ImportNotificationStatus source, ImportNotificationStatus destination)
        {
            Assessment = assessment;
            Source = source;
            Destination = destination;
        }
    }
}