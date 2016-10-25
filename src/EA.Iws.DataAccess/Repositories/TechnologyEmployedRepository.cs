namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class TechnologyEmployedRepository : ITechnologyEmployedRepository
    {
        private readonly IwsContext context;

        public TechnologyEmployedRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<TechnologyEmployed> GetByNotificaitonId(Guid notificationId)
        {
            return await context.TechnologiesEmployed
                .SingleOrDefaultAsync(x => x.NotificationId == notificationId);
        }
    }
}