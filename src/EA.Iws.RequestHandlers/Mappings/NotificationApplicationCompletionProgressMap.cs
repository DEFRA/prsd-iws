namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    public class NotificationApplicationCompletionProgressMap : IMap<NotificationApplication, NotificationApplicationCompletionProgress>
    {
        private readonly INotificationProgressService progressService;

        public NotificationApplicationCompletionProgressMap(INotificationProgressService progressService)
        {
            this.progressService = progressService;
        }

        public NotificationApplicationCompletionProgress Map(NotificationApplication source)
        {
            return progressService.GetNotificationProgressInfo(source.Id);
        }
    }
}
