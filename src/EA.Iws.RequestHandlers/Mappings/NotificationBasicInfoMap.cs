namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Notification;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Requests.Notification;
    using NotificationType = Core.Shared.NotificationType;

    internal class NotificationBasicInfoMap : IMap<NotificationApplication, NotificationBasicInfo>
    {
        public NotificationBasicInfo Map(NotificationApplication source)
        {
            return new NotificationBasicInfo
            {
                NotificationId = source.Id,
                NotificationNumber = source.NotificationNumber,
                NotificationType = (NotificationType)source.NotificationType.Value,
                CompetentAuthority = (CompetentAuthority)source.CompetentAuthority.Value
            };
        }
    }
}