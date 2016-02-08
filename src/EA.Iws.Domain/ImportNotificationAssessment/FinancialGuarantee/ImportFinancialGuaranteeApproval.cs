namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeApproval : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime DecisionDate { get; private set; }

        public string ReferenceNumber { get; private set; }

        protected ImportFinancialGuaranteeApproval()
        {
        }

        public ImportFinancialGuaranteeApproval(Guid importNotificationId, 
            DateTime decisionDate,
            string referenceNumber)
        {
            Guard.ArgumentNotNullOrEmpty(() => referenceNumber, referenceNumber);

            ImportNotificationId = importNotificationId;
            DecisionDate = decisionDate;
            ReferenceNumber = referenceNumber;
        }
    }
}
