namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Security;

    public class SharedUserRepository : ISharedUserRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization authorization;

        public SharedUserRepository(IwsContext context, INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task AddSharedUser(SharedUser sharedUser)
        {
            await authorization.EnsureAccessIsOwnerAsync(sharedUser.NotificationId);

            context.SharedUser.Add(sharedUser);
        }

        public async Task RemoveSharedUser(Guid notificationId, Guid sharedId)
        {
            await authorization.EnsureAccessIsOwnerAsync(notificationId);

            var sharedUser = await context.SharedUser.Where(x => x.NotificationId == notificationId && x.Id == sharedId).SingleAsync();

            context.DeleteOnCommit(sharedUser);
        }

        public async Task<IEnumerable<SharedUser>> GetAllSharedUsers(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return await context.SharedUser.Where(x => x.NotificationId == notificationId).ToArrayAsync();
        }

        public async Task<SharedUser> GetSharedUserById(Guid notificationId, Guid sharedId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return await context.SharedUser.Where(x => x.NotificationId == notificationId && x.Id == sharedId).SingleAsync();
        }
    }
}
