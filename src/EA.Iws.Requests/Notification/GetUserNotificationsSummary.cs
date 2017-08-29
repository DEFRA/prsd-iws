namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetUserNotificationsSummary : IRequest<UserNotificationsSummary>
    {
        public GetUserNotificationsSummary(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
