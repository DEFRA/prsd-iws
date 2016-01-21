namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotificationAssessment)]
    public class GetRefundLimit : IRequest<decimal>
    {
        public Guid NotificationId { get; private set; }

        public GetRefundLimit(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
