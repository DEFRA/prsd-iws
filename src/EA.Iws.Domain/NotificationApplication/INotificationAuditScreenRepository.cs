namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationAuditScreenRepository
    {
        Task<IEnumerable<AuditScreen>> GetNotificationAuditScreens();
    }
}
