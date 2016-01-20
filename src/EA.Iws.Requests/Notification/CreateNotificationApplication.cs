namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanCreateExportNotification)]
    public class CreateNotificationApplication : IRequest<Guid>
    {
        public NotificationType NotificationType { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }
    }
}