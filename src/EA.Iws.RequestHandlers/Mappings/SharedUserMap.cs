namespace EA.Iws.RequestHandlers.Mappings
{
    using DataAccess;
    using Domain;
    using EA.Iws.Core.Notification;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Prsd.Core.Mapper;
    using System;
    using System.Threading.Tasks;

    internal class SharedUserMap : IMap<SharedUser, NotificationSharedUser>
    {
        private readonly INotificationUserRepository notificationUserRepository;

        public SharedUserMap(INotificationUserRepository notificationUserRepository)
        {
            this.notificationUserRepository = notificationUserRepository;
        }

        public NotificationSharedUser Map(SharedUser source)
        {
            User user = Task.Run(() => notificationUserRepository.GetUserByUserId(source.UserId)).Result;
            return new NotificationSharedUser
            {
                Id = source.Id,
                NotificationId = source.NotificationId,
                UserId = source.UserId,
                Email = user.Email
            };
        }
    }
}
