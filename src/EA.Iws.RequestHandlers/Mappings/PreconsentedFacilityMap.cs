namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class PreconsentedFacilityMap : IMap<NotificationApplication, PreconsentedFacilityData>
    {
        public PreconsentedFacilityData Map(NotificationApplication source)
        {
            return new PreconsentedFacilityData
            {
                NotificationId = source.Id,
                IsPreconsentedRecoveryFacility = source.IsPreconsentedRecoveryFacility,
                NotificationType = GetNotificationType(source.NotificationType)
            };
        }

        private static Core.Shared.NotificationType GetNotificationType(NotificationType type)
        {
            Core.Shared.NotificationType notificationType;
            if (Enum.TryParse(type.ToString(), out notificationType))
            {
                return notificationType;
            }
            throw new InvalidOperationException(string.Format("Unknown notification type {0}", type));
        }
    }
}