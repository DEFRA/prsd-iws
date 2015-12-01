namespace EA.Iws.Core.NotificationAssessment
{
    using System;
    using Notification;

    public class NotificationAssessmentSummaryInformationData
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string ActiveSection { get; set; }

        public NotificationStatus Status { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }
    }
}
