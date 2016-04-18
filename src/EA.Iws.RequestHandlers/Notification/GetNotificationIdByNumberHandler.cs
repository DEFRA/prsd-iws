namespace EA.Iws.RequestHandlers.Notification
{
    using System;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationIdByNumberHandler : IRequestHandler<GetNotificationIdByNumber, Guid?>
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetNotificationIdByNumberHandler(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<Guid?> HandleAsync(GetNotificationIdByNumber message)
        {
            return await notificationApplicationRepository.GetIdOrDefault(message.NotificationNumber);
        }
    }
}
