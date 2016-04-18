namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;

    internal class NotificationUserRepository : INotificationUserRepository
    {
        private readonly IwsContext context;

        public NotificationUserRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<User> GetUserByExportNotificationId(Guid notificationId)
        {
            var userId = (await context.GetNotificationApplication(notificationId)).UserId;

            var user = await context.Users.Where(u => u.Id == userId.ToString()).SingleAsync();

            return user;
        }

        public async Task<User> GetUserByUserId(string userId)
        {
            var user = await context.Users.Where(u => u.Id == userId).SingleOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await context.Users.ToArrayAsync();

            return users;
        }
    }
}
