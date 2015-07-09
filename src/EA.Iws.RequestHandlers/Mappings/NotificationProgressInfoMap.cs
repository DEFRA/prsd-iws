namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.Notification;
    using NotificationType = Core.Shared.NotificationType;

    internal class NotificationProgressInfoMap : IMap<NotificationApplication, NotificationProgressInfo>
    {
        private readonly IMap<NotificationApplication, NotificationApplicationCompletionProgress> mapper;

        public NotificationProgressInfoMap(
            IMap<NotificationApplication, NotificationApplicationCompletionProgress> mapper)
        {
            this.mapper = mapper;
        }

        public NotificationProgressInfo Map(NotificationApplication source)
        {
            return new NotificationProgressInfo
            {
                NotificationId = source.Id,
                NotificationNumber = source.NotificationNumber,
                NotificationType = (NotificationType)source.NotificationType.Value,
                CompetentAuthority = (CompetentAuthority)source.CompetentAuthority.Value,
                Progress = mapper.Map(source)
            };
        }
    }
}