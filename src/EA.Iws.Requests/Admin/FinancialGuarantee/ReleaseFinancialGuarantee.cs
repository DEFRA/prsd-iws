namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class ReleaseFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public ReleaseFinancialGuarantee(Guid notificationId, DateTime decisionDate)
        {
            NotificationId = notificationId;
            DecisionDate = decisionDate;
        }
    }
}
