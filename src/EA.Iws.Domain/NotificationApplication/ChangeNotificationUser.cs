namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class ChangeNotificationUser
    {
        private readonly INotificationApplicationRepository repository;

        public ChangeNotificationUser(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task Apply(Guid notificationId, Guid newUserId)
        {
            var notification = await repository.GetById(notificationId);
            notification.ChangeUser(newUserId);
        }
    }
}