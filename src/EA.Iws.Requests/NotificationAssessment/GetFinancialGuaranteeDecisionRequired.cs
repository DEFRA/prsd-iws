namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Prsd.Core.Mediator;

    public class GetFinancialGuaranteeDecisionRequired : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public GetFinancialGuaranteeDecisionRequired(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}