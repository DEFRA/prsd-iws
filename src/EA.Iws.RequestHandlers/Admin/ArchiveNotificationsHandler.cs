namespace EA.Iws.RequestHandlers.Admin
{
    using EA.Iws.Core.Admin.ArchiveNotification;
    using EA.Iws.DataAccess;
    using EA.Iws.Domain;
    using EA.Iws.Requests.Admin.ArchiveNotification;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class ArchiveNotificationsHandler : IRequestHandler<ArchiveNotifications, IList<ArchiveNotificationResult>>
    {
        private readonly IwsContext context;
        private readonly IArchiveNotificationRepository repository;

        public ArchiveNotificationsHandler(IArchiveNotificationRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<IList<ArchiveNotificationResult>> HandleAsync(ArchiveNotifications message)
        {
            var res = new List<ArchiveNotificationResult>();
            foreach (var notificationId in message.NotificationIds)
            {
                var archiveNotificationResult = await repository.ArchiveNotificationAsync(notificationId);
                if (archiveNotificationResult != null)
                {
                    if (archiveNotificationResult.ErrorMessage != null)
                    {
                        //TODO What do we want to do here? Log or just add it anyway?
                    }
                    res.Add(archiveNotificationResult);
                }
            }

            await context.SaveChangesAsync();

            return res;
        }
    }
}