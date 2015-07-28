namespace EA.Iws.Core.Admin
{
    using System;
    using FinancialGuarantee;

    public class FinancialGuaranteeData
    {
        public DateTime? ReceivedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public DateTime? DecisionRequiredDate { get; set; }

        public FinancialGuaranteeStatus Status { get; set; }
    }
}