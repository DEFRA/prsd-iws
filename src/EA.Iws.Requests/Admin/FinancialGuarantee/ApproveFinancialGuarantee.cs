namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class ApproveFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        public int ActiveLoadsPermitted { get; set; }

        public string ReferenceNumber { get; set; }

        public bool IsBlanketbond { get; set; }

        public ApproveFinancialGuarantee(Guid notificationId, 
            DateTime decisionDate,
            DateTime validFrom,
            DateTime validTo,
            string blanketBondReference,
            int activeLoadsPermitted,
            bool isBlanketBond)
        {
            if (!isBlanketBond && validFrom > validTo)
            {
                throw new ArgumentException("Valid from date must be before valid to date.");
            }

            Guard.ArgumentNotZeroOrNegative(() => activeLoadsPermitted, activeLoadsPermitted);

            NotificationId = notificationId;
            DecisionDate = decisionDate;
            ValidFrom = validFrom;
            ValidTo = validTo;
            ActiveLoadsPermitted = activeLoadsPermitted;
            ReferenceNumber = blanketBondReference;
            IsBlanketbond = isBlanketBond;
        }
    }
}
