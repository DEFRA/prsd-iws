namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class RefuseFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public string Reason { get; private set; }

        public RefuseFinancialGuarantee(Guid importNotificationId, DateTime date, string reason)
        {
            Reason = reason;
            DecisionDate = date;
            ImportNotificationId = importNotificationId;
        }
    }
}
