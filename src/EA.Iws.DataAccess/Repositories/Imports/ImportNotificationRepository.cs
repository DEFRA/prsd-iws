namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotification;
    using Prsd.Core;

    internal class ImportNotificationRepository : IImportNotificationRepository
    {
        private readonly ImportNotificationContext context;

        public ImportNotificationRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<bool> NotificationNumberExists(string number)
        {
            Guard.ArgumentNotNull(() => number, number);

            return await context.ImportNotifications.AnyAsync(n => n.NotificationNumber == number);
        }

        public async Task<ImportNotification> Get(Guid id)
        {
            return await context.ImportNotifications.SingleAsync(n => n.Id == id);
        }

        public async Task Add(ImportNotification notification)
        {
            if (await context.ImportNotifications
                .AnyAsync(n => n.NotificationNumber == notification.NotificationNumber))
            {
                throw new InvalidOperationException("Cannot add an import notification with the duplicate number: " + notification.NotificationNumber);
            }

            context.ImportNotifications.Add(notification);
        }

        public async Task<NotificationType> GetTypeById(Guid id)
        {
            return (await context.ImportNotifications.SingleAsync(n => n.Id == id)).NotificationType;
        }
    }
}
