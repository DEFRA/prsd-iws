namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class ApproveFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public string Reference { get; private set; }

        public int ActiveLoadsPermitted { get; private set; }

        public DateTime ValidFrom { get; private set; }
        
        public DateTime ValidTo { get; private set; }

        public ApproveFinancialGuarantee(Guid importNotificationId,
            DateTime decisionDate,
            string reference,
            int activeLoadsPermitted,
            DateTime validFrom,
            DateTime validTo)
        {
            ImportNotificationId = importNotificationId;
            Reference = reference;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ValidFrom = validFrom;
            DecisionDate = decisionDate;
            ValidTo = validTo;
        }
    }
}
