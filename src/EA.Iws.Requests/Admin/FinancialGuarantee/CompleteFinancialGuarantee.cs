namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Prsd.Core.Mediator;

    public class CompleteFinancialGuarantee : IRequest<Unit>
    {
        public CompleteFinancialGuarantee(Guid notificationId, Guid financialGuaranteeId, DateTime completedDate)
        {
            NotificationId = notificationId;
            FinancialGuaranteeId = financialGuaranteeId;
            CompletedDate = completedDate;
        }

        public Guid NotificationId { get; private set; }

        public Guid FinancialGuaranteeId { get; private set; }

        public DateTime CompletedDate { get; private set; }
    }
}