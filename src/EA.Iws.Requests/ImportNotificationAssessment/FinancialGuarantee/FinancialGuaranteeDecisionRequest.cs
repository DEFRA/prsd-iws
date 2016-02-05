namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public abstract class FinancialGuaranteeDecisionRequest : IRequest<bool>
    {
        public DateTime DecisionDate { get; protected set; }
        public Guid ImportNotificationId { get; protected set; }
    }
}
