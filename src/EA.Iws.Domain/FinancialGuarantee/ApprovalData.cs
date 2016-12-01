namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;

    public class ApprovalData
    {
        public DateTime DecisionDate { get; private set; }

        public int ActiveLoadsPermitted { get; private set; }

        public string ReferenceNumber { get; private set; }

        public bool IsBlanketBond { get; private set; }

        public ApprovalData(DateTime decisionDate, string referenceNumber, int activeLoadsPermitted, bool isBlanketBond)
        {
            DecisionDate = decisionDate;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ReferenceNumber = referenceNumber;
            IsBlanketBond = isBlanketBond;
        }
    }
}
