namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class HasApprovedFinancialGuarantee : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public HasApprovedFinancialGuarantee(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
