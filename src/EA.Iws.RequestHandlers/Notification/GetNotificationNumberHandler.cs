namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    public class GetNotificationNumberHandler : IRequestHandler<GetNotificationNumber, string>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetNotificationNumberHandler(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<string> HandleAsync(GetNotificationNumber message)
        {
            return await notificationApplicationRepository.GetNumber(message.NotificationId);
        }
    }
}
