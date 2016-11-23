namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class ReleaseFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public ReleaseFinancialGuarantee(Guid notificationId, Guid financialGuaranteeId,
            DateTime decisionDate)
        {
            NotificationId = notificationId;
            FinancialGuaranteeId = financialGuaranteeId;
            DecisionDate = decisionDate;
        }
    }
}
