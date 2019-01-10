namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    
    public class NotificationAuditScreenRepository : INotificationAuditScreenRepository
    {
        private readonly IwsContext context;

        public NotificationAuditScreenRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<AuditScreen>> GetNotificationAuditScreens()
        {
            return await context.NotificationAuditScreens.ToArrayAsync();
        }
    }
}
