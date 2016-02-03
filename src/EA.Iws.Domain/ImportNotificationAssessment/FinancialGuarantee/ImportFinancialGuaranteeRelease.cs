namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeRelease : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime DecisionDate { get; private set; }

        protected ImportFinancialGuaranteeRelease()
        {
        }

        internal ImportFinancialGuaranteeRelease(Guid importNotificationId, DateTime decisionDate)
        {
            ImportNotificationId = importNotificationId;
            DecisionDate = decisionDate;
        }
    }
}
