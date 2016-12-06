namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotificationAssessment)]
    public class RefuseFinancialGuarantee : IRequest<Unit>
    {
        public DateTime DecisionDate { get; private set; }

        public Guid NotificationId { get; private set; }

        public Guid FinancialGuaranteeId { get; private set; }

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
