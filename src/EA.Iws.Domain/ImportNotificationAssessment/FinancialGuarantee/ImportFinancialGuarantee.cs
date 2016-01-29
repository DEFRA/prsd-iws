namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core.Domain;

    public class ImportFinancialGuarantee : Entity
    {
        public Guid ImportNotificationId { get; private set; }

        public FinancialGuaranteeStatus Status { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        protected ImportFinancialGuarantee()
        {
        }

        public ImportFinancialGuarantee(Guid importNotificationId, DateTime receivedDate)
        {
            ImportNotificationId = importNotificationId;
            ReceivedDate = receivedDate;
            CreatedDate = DateTimeOffset.UtcNow;
        }
    }
}
