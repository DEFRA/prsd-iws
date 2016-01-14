namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class PreconsentedFacilityMap : IMap<NotificationApplication, PreconsentedFacilityData>
    {
        private readonly IFacilityRepository facilityRepository;

        public PreconsentedFacilityMap(IFacilityRepository facilityRepository)
        {
            this.facilityRepository = facilityRepository;
        }

        public PreconsentedFacilityData Map(NotificationApplication source)
        {
            var facilityCollection = Task.Run(() => facilityRepository.GetByNotificationId(source.Id)).Result;

            return new PreconsentedFacilityData
            {
                NotificationId = source.Id,
                IsPreconsentedRecoveryFacility = facilityCollection.AllFacilitiesPreconsented,
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