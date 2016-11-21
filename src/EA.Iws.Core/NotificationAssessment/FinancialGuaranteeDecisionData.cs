namespace EA.Iws.Core.NotificationAssessment
{
    using System;

    public class FinancialGuaranteeDecisionData
    {
        public Guid NotificationId { get; set; }

        public DateTime Date { get; set; }

        public string Decision { get; set; }
    }
}