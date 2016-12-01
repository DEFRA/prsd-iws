namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class ReleaseFinancialGuarantee : IRequest<Unit>
    {
        public DateTime DecisionDate { get; private set; }

        public Guid NotificationId { get; private set; }

        public Guid FinancialGuaranteeId { get; private set; }

        public ReleaseFinancialGuarantee(Guid notificationId, Guid financialGuaranteeId,
            DateTime decisionDate)
        {
            NotificationId = notificationId;
            FinancialGuaranteeId = financialGuaranteeId;
            DecisionDate = decisionDate;
        }
    }
}
