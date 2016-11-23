namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class ApproveFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public int ActiveLoadsPermitted { get; set; }

        public string ReferenceNumber { get; set; }

        public bool IsBlanketbond { get; set; }

        public ApproveFinancialGuarantee(Guid notificationId, 
            Guid financialGuaranteeId,
            DateTime decisionDate,
            string blanketBondReference,
            int activeLoadsPermitted,
            bool isBlanketBond)
        {
            Guard.ArgumentNotZeroOrNegative(() => activeLoadsPermitted, activeLoadsPermitted);

            NotificationId = notificationId;
            FinancialGuaranteeId = financialGuaranteeId;
            DecisionDate = decisionDate;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ReferenceNumber = blanketBondReference;
            IsBlanketbond = isBlanketBond;
        }
    }
}
