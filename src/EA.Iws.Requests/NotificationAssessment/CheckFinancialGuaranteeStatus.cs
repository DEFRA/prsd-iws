namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class CheckFinancialGuaranteeStatus : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public CheckFinancialGuaranteeStatus(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
