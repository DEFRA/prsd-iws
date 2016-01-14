namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Domain.NotificationApplication;

    [AutoRegister]
    internal class NotificationUserService : INotificationUserService
    {
        private readonly INotificationApplicationRepository repository;

        public NotificationUserService(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task ChangeNotificationUser(Guid notificationId, Guid newUserId)
        {
            var notification = await repository.GetById(notificationId);
            notification.ChangeUser(newUserId);
        }
    }
}
