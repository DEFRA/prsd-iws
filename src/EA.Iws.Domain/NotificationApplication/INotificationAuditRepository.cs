namespace EA.Iws.Domain.NotificationApplication
{
    using Core.Notification.Audit;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationAuditRepository
    {
        Task AddNotificationAudit(Audit notificationAudit);

        Task<IEnumerable<Audit>> GetNotificationAuditsById(Guid notificationId);
    }
}
