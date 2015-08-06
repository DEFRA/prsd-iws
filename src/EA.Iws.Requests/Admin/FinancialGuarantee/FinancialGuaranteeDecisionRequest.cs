namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Prsd.Core.Mediator;

    public abstract class FinancialGuaranteeDecisionRequest : IRequest<bool>
    {
        public DateTime DecisionDate { get; protected set; }
        public Guid NotificationId { get; protected set; }
    }
}
