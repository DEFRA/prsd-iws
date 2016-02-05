namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class CheckNotificationNumberUniqueHandler : IRequestHandler<CheckNotificationNumberUnique, bool>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public CheckNotificationNumberUniqueHandler(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<bool> HandleAsync(CheckNotificationNumberUnique message)
        {
            return !await notificationApplicationRepository.NotificationNumberExists(
                message.NotificationNumber,
                message.CompetentAuthority);
        }
    }
}