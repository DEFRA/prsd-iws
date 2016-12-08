namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationTypeHandler : IRequestHandler<GetNotificationType, NotificationType>
    {
        private readonly INotificationApplicationRepository repository;

        public GetNotificationTypeHandler(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<NotificationType> HandleAsync(GetNotificationType message)
        {
            return await repository.GetNotificationType(message.NotificationId);
        }
    }
}
