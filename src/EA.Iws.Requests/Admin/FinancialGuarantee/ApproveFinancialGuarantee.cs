namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class ApproveFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public DateTime ApprovedFrom { get; private set; }

        public DateTime ApprovedTo { get; private set; }

        public int ActiveLoadsPermitted { get; set; }

        public decimal AmountOfCoverProvided { get; set; }

        public string BlanketBondReference { get; set; }

        public ApproveFinancialGuarantee(Guid notificationId, 
            DateTime decisionDate,
            DateTime approvedFrom,
            DateTime approvedTo,
            string blanketBondReference,
            int activeLoadsPermitted,
            decimal amountOfCoverProvided)
        {
            if (approvedFrom > approvedTo)
            {
                throw new ArgumentException("Approved from date must be before approved to date.");
            }

            Guard.ArgumentNotZeroOrNegative(() => activeLoadsPermitted, activeLoadsPermitted);

            NotificationId = notificationId;
            DecisionDate = decisionDate;
            ApprovedFrom = approvedFrom;
            ApprovedTo = approvedTo;
            ActiveLoadsPermitted = activeLoadsPermitted;
            BlanketBondReference = blanketBondReference;
            AmountOfCoverProvided = amountOfCoverProvided;
        }
    }
}
