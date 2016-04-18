namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationApplicationOverviewRepository
    {
        Task<NotificationApplicationOverview> GetById(Guid notificationId);
    }
}
