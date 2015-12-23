namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;

    public class ApproveDates
    {
        public DateTime DecisionDate { get; private set; }

        public DateTime ApprovedFrom { get; private set; }

        public DateTime ApprovedTo { get; private set; }

        public int ActiveLoadsPermitted { get; private set; }

        public decimal AmountOfCoverProvided { get; private set; }

        public string BlanketBondReference { get; private set; }

        public ApproveDates(DateTime decisionDate, DateTime approvedFrom, DateTime approvedTo, string blanketBondReference, int activeLoadsPermitted, decimal amountOfCoverProvided)
        {
            if (approvedFrom > approvedTo)
            {
                throw new InvalidOperationException("Approved from cannot be after approved to.");
            }

            DecisionDate = decisionDate;
            ApprovedFrom = approvedFrom;
            ApprovedTo = approvedTo;
            ActiveLoadsPermitted = activeLoadsPermitted;
            AmountOfCoverProvided = amountOfCoverProvided;
            BlanketBondReference = blanketBondReference;
        }
    }
}
