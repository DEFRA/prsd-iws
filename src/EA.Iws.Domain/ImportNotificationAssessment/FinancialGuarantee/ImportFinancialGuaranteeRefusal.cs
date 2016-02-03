namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class ImportFinancialGuaranteeRefusal : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime DecisionDate { get; private set; }

        public string Reason { get; private set; }

        protected ImportFinancialGuaranteeRefusal()
        {
        }

        internal ImportFinancialGuaranteeRefusal(Guid importNotificationId, DateTime decisonDate, string reason)
        {
            Guard.ArgumentNotNullOrEmpty(() => reason, reason);

            ImportNotificationId = importNotificationId;
            Reason = reason;
            DecisionDate = decisonDate;
        }
    }
}
