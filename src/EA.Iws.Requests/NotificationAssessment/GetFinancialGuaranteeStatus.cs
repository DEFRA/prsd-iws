namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetFinancialGuaranteeStatus : IRequest<FinancialGuaranteeStatus>
    {
        public Guid NotificationId { get; private set; }

        public GetFinancialGuaranteeStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
