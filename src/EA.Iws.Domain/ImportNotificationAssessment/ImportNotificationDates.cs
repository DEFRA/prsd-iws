﻿namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Domain;

    public class ImportNotificationDates : Entity
    {
        public DateTime? NotificationReceivedDate { get; internal set; }

        public DateTime? PaymentReceivedDate { get; internal set; }

        public DateTime? AssessmentStartedDate { get; internal set; }

        public string NameOfOfficer { get; internal set; }

        public DateTime? NotificationCompletedDate { get; internal set; }

        public DateTime? AcknowledgedDate { get; internal set; }

        public DateTime? DecisionRequiredByDate { get; internal set; }

        public DateTime? ConsentedDate { get; internal set; }

        public DateTime? ConsentWithdrawnDate { get; internal set; }

        public string ConsentWithdrawnReasons { get; internal set; }

        public DateTime? FileClosedDate { get; internal set; }

        public ImportNotificationStatus? StatusAtFileClosed { get; internal set; }

        public string ArchiveReference { get; internal set; }

        internal ImportNotificationDates()
        {
        }
    }
}
