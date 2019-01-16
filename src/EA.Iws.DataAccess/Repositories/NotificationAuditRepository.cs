namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;

    public class NotificationAuditRepository : INotificationAuditRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public NotificationAuditRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task AddNotificationAudit(Audit notificationAudit)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationAudit.NotificationId);

            this.context.NotificationAudit.Add(notificationAudit);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Audit>> GetNotificationAuditsById(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);

            return await this.context.NotificationAudit.Where(p => p.NotificationId == notificationId).ToArrayAsync();
        }
    }
}
