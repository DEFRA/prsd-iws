namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class RefuseFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public string ReasonForRefusal { get; private set; }

        public RefuseFinancialGuarantee(Guid notificationId, Guid financialGuaranteeId,
            DateTime decisionDate, string reasonForRefusal)
        {
            Guard.ArgumentNotNullOrEmpty(() => reasonForRefusal, reasonForRefusal);

            NotificationId = notificationId;
            FinancialGuaranteeId = financialGuaranteeId;
            DecisionDate = decisionDate;
            ReasonForRefusal = reasonForRefusal;
        }
    }
}
