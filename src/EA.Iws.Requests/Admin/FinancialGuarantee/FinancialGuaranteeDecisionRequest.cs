namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public abstract class FinancialGuaranteeDecisionRequest : IRequest<bool>
    {
        public DateTime DecisionDate { get; protected set; }

        public Guid NotificationId { get; protected set; }

        public Guid FinancialGuaranteeId { get; protected set; }
    }
}
