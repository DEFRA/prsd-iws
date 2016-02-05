namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class ApproveBlanketBondFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public string BlanketBondReference { get; private set; }

        public int ActiveLoadsPermitted { get; private set; }

        public DateTime ValidFrom { get; private set; }

        public ApproveBlanketBondFinancialGuarantee(Guid importNotificationId, 
            DateTime decisionDate, 
            string blanketBondReference, 
            int activeLoadsPermitted, 
            DateTime validFrom)
        {
            ImportNotificationId = importNotificationId;
            BlanketBondReference = blanketBondReference;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ValidFrom = validFrom;
            DecisionDate = decisionDate;
        }
    }
}