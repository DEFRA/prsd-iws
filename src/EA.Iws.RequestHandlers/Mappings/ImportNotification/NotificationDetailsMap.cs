namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;

    internal class NotificationDetailsMap : IMapWithParameter<ImportNotification, ImportNotificationStatus, NotificationDetails>
    {
        public NotificationDetails Map(ImportNotification source, ImportNotificationStatus parameter)
        {
            return new NotificationDetails
            {
                ImportNotificationId = source.Id,
                NotificationType = source.NotificationType,
                NotificationNumber = source.NotificationNumber,
                CompetentAuthority = source.CompetentAuthority,
                Status = parameter
            };
        }
    }
}