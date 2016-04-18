namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationMovementsSummaryRepository
    {
        Task<NotificationMovementsSummary> GetById(Guid notificationId);
    }
}