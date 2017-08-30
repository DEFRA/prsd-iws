namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Notification;
    using NotificationAssessment;

    public class KeyDatesData
    {
        public DateTime? NotificationReceived { get; set; }

        public DateTime? PaymentReceived { get; set; }

        public DateTime? AssessmentStarted { get; set; }

        public string NameOfOfficer { get; set; }

        public bool IsPaymentComplete { get; set; }

        public DateTime? NotificationCompletedDate { get; set; }

        public DateTime? AcknowlegedDate { get; set; }

        public DateTime? DecisionRequiredByDate { get; set; }

        public bool IsInterim { get; set; }

        public DateTime? FileClosedDate { get; set; }

        public string ArchiveReference { get; set; }

        public IList<NotificationAssessmentDecision> DecisionHistory { get; set; } 

        public bool IsLocalAreaSet { get; set; }

        public UKCompetentAuthority CompententAuthority { get; set; }

        public ImportNotificationStatus Status { get; set; }
    }
}
