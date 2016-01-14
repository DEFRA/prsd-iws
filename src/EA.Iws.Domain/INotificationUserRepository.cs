namespace EA.Iws.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationUserRepository
    {
        Task<User> GetUserByExportNotificationId(Guid notificationId);

        Task<User> GetUserByUserId(string userId);

        Task<IEnumerable<User>> GetAllUsers();
    }
}
