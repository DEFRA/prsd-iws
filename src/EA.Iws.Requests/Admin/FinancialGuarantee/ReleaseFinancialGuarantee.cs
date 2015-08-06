namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;

    public class ReleaseFinancialGuarantee : FinancialGuaranteeDecisionRequest
    {
        public ReleaseFinancialGuarantee(Guid notificationId, DateTime decisionDate)
        {
            NotificationId = notificationId;
            DecisionDate = decisionDate;
        }
    }
}
