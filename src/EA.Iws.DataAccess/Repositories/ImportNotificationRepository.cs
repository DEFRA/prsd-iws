namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Prsd.Core;

    public class ImportNotificationRepository : IImportNotificationRepository
    {
        private readonly IwsContext context;

        public ImportNotificationRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> NotificationNumberExists(string number)
        {
            Guard.ArgumentNotNull(() => number, number);

            var numberWithSpacesRemoved = number.Replace(" ", string.Empty);

            return await context.ImportNotifications.AnyAsync(n => n.NotificationNumber == number);
        }

        public async Task<ImportNotification> GetByImportNotificationId(Guid id)
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

        public async Task<IEnumerable<ImportNotification>> SearchByNumber(string number)
        {
            var notifications =
                await context.ImportNotifications.Where(n => n.NotificationNumber.Contains(number))
                .ToArrayAsync();

            return notifications;
        }
    }
}
