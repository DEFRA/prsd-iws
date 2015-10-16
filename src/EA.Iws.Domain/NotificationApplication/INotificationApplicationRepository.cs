namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationApplicationRepository
    {
        Task<NotificationApplication> GetById(Guid id);
        Task<NotificationApplication> GetByMovementId(Guid movementId);
    }
}