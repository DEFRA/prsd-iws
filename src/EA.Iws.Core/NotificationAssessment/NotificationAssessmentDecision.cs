namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class NotificationAssessmentDecision
    {
        public Guid NotificationId { get; set; }

        public DateTime Date { get; set; }

        public DateTime? ConsentedFrom { get; set; }

        public DateTime? ConsentedTo { get; set; }

        public NotificationStatus Status { get; set; }

        public NotificationAssessmentDecision(Guid notificationId, DateTime date, DateTime? consentedFrom, DateTime? consentedTo, NotificationStatus status)
        {
            NotificationId = notificationId;
            Date = date;
            ConsentedFrom = consentedFrom;
            ConsentedTo = consentedTo;
            Status = status;
        }
    }
}
