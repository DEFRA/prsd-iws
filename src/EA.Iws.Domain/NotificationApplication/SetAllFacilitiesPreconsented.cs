namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;

    [AutoRegister]
    public class SetAllFacilitiesPreconsented
    {
        private readonly IFacilityRepository facilityRepository;

        public SetAllFacilitiesPreconsented(IFacilityRepository facilityRepository)
        {
            this.facilityRepository = facilityRepository;
        }

        public async Task SetForNotification(NotificationApplication notification, bool allFacilitiesPreconsented)
        {
            if (notification.NotificationType != Core.Shared.NotificationType.Recovery)
            {
                throw new InvalidOperationException(String.Format(
                    "Can't set pre-consented recovery facility as notification type is not Recovery for notification: {0}", notification.Id));
            }

            var facilityCollection = await facilityRepository.GetByNotificationId(notification.Id);

            facilityCollection.AllFacilitiesPreconsented = allFacilitiesPreconsented;
        }
    }
}