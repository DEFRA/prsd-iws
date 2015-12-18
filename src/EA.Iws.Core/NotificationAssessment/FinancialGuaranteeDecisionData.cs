namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class FinancialGuaranteeDecisionData
    {
        public Guid NotificationId { get; set; }

        public DateTime Date { get; set; }

        public string Decision { get; set; }

        public DateTime? ApprovedFrom { get; set; }

        public DateTime? ApprovedTo { get; set; }
    }
}