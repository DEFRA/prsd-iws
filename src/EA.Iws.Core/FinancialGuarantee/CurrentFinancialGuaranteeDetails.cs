namespace EA.Iws.Core.FinancialGuarantee
{
    using System;

    public class CurrentFinancialGuaranteeDetails
    {
        public Guid NotificationId { get; set; }

        public Guid FinancialGuaranteeId { get; set; }

        public string Decision { get; set; }

        public DateTime DecisionDate { get; set; }

        public bool IsBlanketBond { get; set; }

        public string ReferenceNumber { get; set; }

        public int ActiveLoadsPermitted { get; set; }
    }
}