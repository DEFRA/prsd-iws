namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain;

    internal class InternalUserRepository : IInternalUserRepository
    {
        private readonly IwsContext context;

        public InternalUserRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<InternalUser> GetByUserId(string userId)
        {
            return await context.InternalUsers.SingleAsync(u => u.UserId == userId);
        }

        public async Task<InternalUser> GetByUserId(Guid userId)
        {
            return await GetByUserId(userId.ToString());
        }

        public async Task<bool> IsUserInternal(string userId)
        {
            return await context.InternalUsers.AnyAsync(u => u.UserId == userId.ToString());
        }

        public async Task<bool> IsUserInternal(Guid userId)
        {
            return await IsUserInternal(userId);
        }
    }
}
