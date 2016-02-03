namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface INotificationApplicationRepository
    {
        Task<NotificationApplication> GetById(Guid id);

        Task<NotificationApplication> GetByMovementId(Guid movementId);

        Task<string> GetNumber(Guid id);

        Task<Guid?> GetIdOrDefault(string number);

        void Add(NotificationApplication notification);

        Task<bool> NotificationNumberExists(int number, UKCompetentAuthority competentAuthority);
    }
}