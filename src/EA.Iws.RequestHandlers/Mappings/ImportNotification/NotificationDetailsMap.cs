namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;

    internal class NotificationDetailsMap : IMap<ImportNotification, NotificationDetails>
    {
        public NotificationDetails Map(ImportNotification source)
        {
            return new NotificationDetails
            {
                ImportNotificationId = source.Id,
                NotificationType = source.NotificationType,
                NotificatioNumber = source.NotificationNumber
            };
        }
    }
}