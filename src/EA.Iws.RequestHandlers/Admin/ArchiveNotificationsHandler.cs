namespace EA.Iws.RequestHandlers.Admin
{
    using EA.Iws.Domain;
    using EA.Iws.Requests.Admin.ArchiveNotification;
    using EA.Iws.Requests.Notification;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class ArchiveNotificationsHandler : IRequestHandler<ArchiveNotifications, IList<NotificationArchiveSummaryData>>
    {
        private readonly IArchiveNotificationRepository repository;

        public ArchiveNotificationsHandler(IArchiveNotificationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IList<NotificationArchiveSummaryData>> HandleAsync(ArchiveNotifications message)
        {
            var res = message.Notifications;
            foreach (var notification in res)
            {
                try
                {
                    var archiveNotificationResult = await repository.ArchiveNotificationAsync(notification.Id);
                    if (archiveNotificationResult != null)
                    {
                        if (archiveNotificationResult == "false")
                        {
                            notification.IsArchived = false;
                        }
                        else
                        {
                            notification.IsArchived = true;
                        }
                    }
                }
                catch
                {
                    notification.IsArchived = false;
                }
            }
            return res;
        }
    }
}