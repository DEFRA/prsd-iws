namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System;

    public class ImportInterimStatus
    {
        public Guid NotificationId { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public bool? IsInterim { get; set; }

    }
}
