namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanCreateLegacyExportNotification)]
    public class CreateLegacyNotificationApplication : IRequest<Guid>
    {
        public NotificationType NotificationType { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public int Number { get; set; }
    }
}