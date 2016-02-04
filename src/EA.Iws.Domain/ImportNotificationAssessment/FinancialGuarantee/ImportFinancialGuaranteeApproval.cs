namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeApproval : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime DecisionDate { get; private set; }

        public DateTime ValidFrom { get; private set; }

        public DateTime? ValidTo { get; private set; }

        public int ActiveLoadsPermitted { get; private set; }

        public string ReferenceNumber { get; private set; }

        public bool IsBlanketBond { get; private set; }

        protected ImportFinancialGuaranteeApproval()
        {
        }

        private ImportFinancialGuaranteeApproval(Guid importNotificationId, 
            DateTime decisionDate, 
            DateTime validFrom, 
            DateTime? validTo, 
            int activeLoadsPermitted, 
            string referenceNumber,
            bool isBlanketBond)
        {
            ImportNotificationId = importNotificationId;
            DecisionDate = decisionDate;
            ValidFrom = validFrom;
            ValidTo = validTo;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ReferenceNumber = referenceNumber;
            IsBlanketBond = isBlanketBond;
        }

        internal static ImportFinancialGuaranteeApproval CreateApproval(Guid importNotificationId, DateTime decisionDate, DateRange validDates,
            int activeLoads, string reference)
        {
            Guard.ArgumentNotNullOrEmpty(() => reference, reference);
            Guard.ArgumentNotNull(() => validDates, validDates);
            Guard.ArgumentNotZeroOrNegative(() => activeLoads, activeLoads);

            return new ImportFinancialGuaranteeApproval(importNotificationId, 
                decisionDate, 
                validDates.From, 
                validDates.To, 
                activeLoads, 
                reference, 
                false);
        }

        internal static ImportFinancialGuaranteeApproval CreateBlanketBondApproval(Guid importNotificationId, DateTime decisionDate, DateTime validFrom,
            int activeLoads, string reference)
        {
            Guard.ArgumentNotNullOrEmpty(() => reference, reference);
            Guard.ArgumentNotZeroOrNegative(() => activeLoads, activeLoads);

            return new ImportFinancialGuaranteeApproval(importNotificationId,
                decisionDate,
                validFrom,
                null,
                activeLoads,
                reference,
                true);
        }
    }
}
