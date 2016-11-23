namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;

    public class FinancialGuaranteeDecision
    {
        public Guid NotificationId { get; set; }

        public DateTime Date { get; set; }

        public FinancialGuaranteeStatus Status { get; set; }
    }
}