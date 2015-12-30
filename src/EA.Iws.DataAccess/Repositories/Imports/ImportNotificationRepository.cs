namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.ImportNotification;
    using Prsd.Core;
    using Prsd.Core.Domain;

    internal class ImportNotificationRepository : IImportNotificationRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IUserContext userContext;
        private readonly IInternalUserRepository internalUserRepository;

        public ImportNotificationRepository(ImportNotificationContext context, IUserContext userContext, IInternalUserRepository internalUserRepository)
        {
            this.context = context;
            this.userContext = userContext;
            this.internalUserRepository = internalUserRepository;
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

        public async Task<IEnumerable<ImportNotification>> SearchByNumber(string number)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);
            var competentAuthority = user.CompetentAuthority.Value;

            var notifications =
                await context.ImportNotifications
                .Where(n => n.CompetentAuthority.Value == competentAuthority && n.NotificationNumber.Contains(number))
                .ToArrayAsync();

            return notifications;
        }
    }
}
