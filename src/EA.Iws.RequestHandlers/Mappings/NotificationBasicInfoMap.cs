namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class NotificationBasicInfoMap : IMap<NotificationApplication, NotificationBasicInfo>
    {
        public NotificationBasicInfo Map(NotificationApplication source)
        {
            return new NotificationBasicInfo
            {
                NotificationId = source.Id,
                NotificationNumber = source.NotificationNumber,
                NotificationType = source.NotificationType,
                CompetentAuthority = source.CompetentAuthority
            };
        }
    }
}