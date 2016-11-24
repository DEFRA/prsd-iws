namespace EA.Iws.Core.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Admin;

    public class NotificationAssessmentDecisionData
    {
        public Guid NotificationId { get; set; }

        public NotificationStatus Status { get; set; }

        public IList<NotificationAssessmentDecisionRecord> StatusHistory { get; set; }

        public IEnumerable<DecisionType> AvailableDecisions { get; set; }

        public string ReasonForObjection { get; set; }

        public string ReasonWithdrawn { get; set; }

        public DateTime AcknowledgedOnDate { get; set; }

        public bool IsPreconsented { get; set; }

        public DateTime? ConsentedDate { get; set; }

        public DateTime? NotificationReceivedDate { get; set; }
    }
}
