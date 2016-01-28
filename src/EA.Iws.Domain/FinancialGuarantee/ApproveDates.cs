namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;

    public class ApproveDates
    {
        public DateTime DecisionDate { get; private set; }

        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        public int ActiveLoadsPermitted { get; private set; }

        public string ReferenceNumber { get; private set; }

        public bool IsBlanketBond { get; private set; }

        public ApproveDates(DateTime decisionDate, DateTime validFrom, DateTime validTo, string referenceNumber, int activeLoadsPermitted, bool isBlanketBond)
        {
            if (!isBlanketBond && validFrom > validTo)
            {
                throw new InvalidOperationException("Valid from cannot be after valid to.");
            }

            DecisionDate = decisionDate;
            ValidFrom = validFrom;
            ValidTo = validTo;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ReferenceNumber = referenceNumber;
            IsBlanketBond = isBlanketBond;
        }
    }
}
