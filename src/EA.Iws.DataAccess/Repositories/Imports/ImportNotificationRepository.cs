namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportNotification;
    using Domain.Security;
    using Prsd.Core;

    internal class ImportNotificationRepository : IImportNotificationRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportNotificationRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<bool> NotificationNumberExists(string number)
        {
            Guard.ArgumentNotNull(() => number, number);

            return await context.ImportNotifications.AnyAsync(n => n.NotificationNumber == number);
        }

        public async Task<ImportNotification> Get(Guid id)
        {
            await authorization.EnsureAccessAsync(id);
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
            await authorization.EnsureAccessAsync(id);
            return (await context.ImportNotifications.SingleAsync(n => n.Id == id)).NotificationType;
        }
    }
}
