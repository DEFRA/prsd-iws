namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;

    public class GetFinancialGuaranteeStatus : IRequest<FinancialGuaranteeStatus>
    {
        public Guid NotificationId { get; private set; }

        public GetFinancialGuaranteeStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
