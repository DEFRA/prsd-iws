namespace EA.Iws.Domain.NotificationApplication
{
    using Core.ComponentRegistration;
    using System;
    using System.Threading.Tasks;

    [AutoRegister]
    public class AddNotificationSharedUser
    {
        private readonly INotificationApplicationRepository repository;

        public AddNotificationSharedUser(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task Apply(Guid notificationId, string sharedUserId)
        {
            var notification = await repository.GetById(notificationId);
            notification.AddSharedUser(sharedUserId);
        }
    }
}
