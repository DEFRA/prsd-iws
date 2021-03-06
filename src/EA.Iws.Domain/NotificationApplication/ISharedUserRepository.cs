﻿namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface ISharedUserRepository
    {
        Task AddSharedUser(SharedUser sharedUser);

        Task RemoveSharedUser(Guid notificationId, Guid sharedId);

        Task<IEnumerable<SharedUser>> GetAllSharedUsers(Guid notificationId);

        Task<SharedUser> GetSharedUserById(Guid notificationId, Guid sharedId);
    }
}
