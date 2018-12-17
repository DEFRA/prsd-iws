namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class SharedUserRepository : ISharedUserRepository
    {
        private readonly IwsContext context;

        public SharedUserRepository(IwsContext context)
        {
            this.context = context;
        }

        public void AddSharedUser(SharedUser sharedUser)
        {
            this.context.SharedUser.Add(sharedUser);
        }

        public async Task RemoveSharedUser(Guid notificationId, Guid sharedId)
        {
             var sharedUser = await context.SharedUser.Where(x => x.NotificationId == notificationId && x.Id == sharedId).SingleAsync();

            context.DeleteOnCommit(sharedUser);
        }

        public async Task<IEnumerable<SharedUser>> GetAllSharedUsers(Guid notificationId)
        {
            return await context.SharedUser.Where(x => x.NotificationId == notificationId).ToArrayAsync();
        }

        public async Task<SharedUser> GetSharedUserById(Guid notificationId, Guid sharedId)
        {
            return await context.SharedUser.Where(x => x.NotificationId == notificationId && x.Id == sharedId).SingleAsync();
        }
    }
}
