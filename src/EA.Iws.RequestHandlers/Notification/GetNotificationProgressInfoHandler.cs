namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GetNotificationProgressInfoHandler :
        IRequestHandler<GetNotificationProgressInfo, NotificationApplicationCompletionProgress>
    {
        private readonly INotificationProgressService progressService;

        public GetNotificationProgressInfoHandler(INotificationProgressService progressService)
        {
            this.progressService = progressService;
        }

        public Task<NotificationApplicationCompletionProgress> HandleAsync(GetNotificationProgressInfo message)
        {
            return Task.FromResult(progressService.GetNotificationProgressInfo(message.NotificationId));
        }
    }
}