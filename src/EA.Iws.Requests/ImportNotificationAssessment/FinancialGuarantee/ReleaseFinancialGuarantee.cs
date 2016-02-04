namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class ReleaseFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public ReleaseFinancialGuarantee(Guid importNotificationId, DateTime date)
        {
            DecisionDate = date;
            ImportNotificationId = importNotificationId;
        }
    }
}
