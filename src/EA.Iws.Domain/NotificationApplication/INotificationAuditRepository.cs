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

        Task<IEnumerable<Audit>> GetPagedNotificationAuditsById(Guid notificationId, int pageNumber, int pageSize, int screen, DateTime startDate, DateTime endDate);

        Task<int> GetTotalNumberOfNotificationAudits(Guid notificationId);
        Task<int> GetTotalNumberOfFilteredAudits(Guid notificationId, int screen, DateTime startDate, DateTime endDate);
    }
}
