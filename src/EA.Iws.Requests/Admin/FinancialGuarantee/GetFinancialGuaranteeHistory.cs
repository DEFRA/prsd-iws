namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetFinancialGuaranteeHistory : IRequest<FinancialGuaranteeData[]>
    {
        public GetFinancialGuaranteeHistory(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}