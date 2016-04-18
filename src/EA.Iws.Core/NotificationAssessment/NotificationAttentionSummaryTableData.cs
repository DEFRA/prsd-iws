namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class NotificationAttentionSummaryTableData
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public string Officer { get; set; }

        public DateTime AcknowledgedDate { get; set; }

        public DateTime DecisionRequiredDate { get; set; }

        public string DaysRemaining { get; set; }
    }
}