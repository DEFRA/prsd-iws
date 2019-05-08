namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.ImportNotification;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetNotificationDetailsHandler : IRequestHandler<GetNotificationDetails, NotificationDetails>
    {
        private readonly IImportNotificationRepository notificationRepository;

        public GetNotificationDetailsHandler(IImportNotificationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<NotificationDetails> HandleAsync(GetNotificationDetails message)
        {
            return await notificationRepository.GetDetails(message.ImportNotificationId);
        }
    }
}