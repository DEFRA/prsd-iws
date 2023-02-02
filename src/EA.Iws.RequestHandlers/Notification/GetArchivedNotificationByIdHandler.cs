namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetArchivedNotificationByIdHandler : IRequestHandler<GetArchivedNotificationById, bool>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetArchivedNotificationByIdHandler(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<bool> HandleAsync(GetArchivedNotificationById message)
        {
            return await notificationApplicationRepository.GetIsArchived(message.NotificationId);
        }
    }
}
