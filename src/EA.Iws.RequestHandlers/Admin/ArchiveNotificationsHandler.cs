namespace EA.Iws.RequestHandlers.Admin
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain;
    using EA.Iws.Requests.Admin.ArchiveNotification;
    using EA.Iws.Requests.Notification;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class ArchiveNotificationsHandler : IRequestHandler<ArchiveNotifications, IList<NotificationArchiveSummaryData>>
    {
        private readonly IwsContext context;
        private readonly IArchiveNotificationRepository repository;

        public ArchiveNotificationsHandler(IArchiveNotificationRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<IList<NotificationArchiveSummaryData>> HandleAsync(ArchiveNotifications message)
        {
            var res = message.Notifications;
            foreach (var notification in res)
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

            await context.SaveChangesAsync();

            return res;
        }
    }
}