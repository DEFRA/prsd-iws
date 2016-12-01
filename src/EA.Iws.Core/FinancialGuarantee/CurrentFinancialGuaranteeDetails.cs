namespace EA.Iws.Core.FinancialGuarantee
{
    using System;

    public class CurrentFinancialGuaranteeDetails
    {
        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        public DateTime ReceivedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public FinancialGuaranteeStatus Status { get; set; }

        public FinancialGuaranteeDecision Decision { get; set; }
    }
}