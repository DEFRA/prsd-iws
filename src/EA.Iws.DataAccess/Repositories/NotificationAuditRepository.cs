namespace EA.Iws.DataAccess.Repositories
{
    using Domain.NotificationApplication;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    public class NotificationAuditRepository : INotificationAuditRepository
    {
        private readonly IwsContext context;

        public NotificationAuditRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task AddNotificationAudit(Audit notificationAudit)
        {
            this.context.NotificationAudit.Add(notificationAudit);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Audit>> GetNotificationAuditsById(Guid notificationId)
        {
            return await this.context.NotificationAudit.Where(p => p.NotificationId == notificationId).ToArrayAsync();
        }
    }
}
