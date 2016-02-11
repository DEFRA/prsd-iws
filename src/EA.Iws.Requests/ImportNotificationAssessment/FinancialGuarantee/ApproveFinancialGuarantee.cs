namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class ApproveFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public string Reference { get; private set; }

        public ApproveFinancialGuarantee(Guid importNotificationId,
            DateTime decisionDate,
            string reference)
        {
            ImportNotificationId = importNotificationId;
            Reference = reference;
            DecisionDate = decisionDate;
        }
    }
}
