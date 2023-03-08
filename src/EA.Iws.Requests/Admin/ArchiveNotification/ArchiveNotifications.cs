namespace EA.Iws.Requests.Admin.ArchiveNotification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Requests.Notification;
    using Prsd.Core.Mediator;
    using System.Collections.Generic;

    [RequestAuthorization(GeneralPermissions.CanViewSearchResults)]
    public class ArchiveNotifications : IRequest<IList<NotificationArchiveSummaryData>>
    {
        public List<NotificationArchiveSummaryData> Notifications { get; private set; }

        public ArchiveNotifications(List<NotificationArchiveSummaryData> notifications)
        {
            Notifications = notifications;
        }
    }
}