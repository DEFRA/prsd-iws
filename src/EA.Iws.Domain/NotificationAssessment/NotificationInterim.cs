namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using NotificationApplication;

    [AutoRegister]
    public class NotificationInterim
    {
        private readonly IFacilityRepository facilityRepository;

        public NotificationInterim(IFacilityRepository facilityRepository)
        {
            this.facilityRepository = facilityRepository;
        }

        public async Task SetValue(Guid notificationId, bool isInterim)
        {
            var facilityCollection = await facilityRepository.GetByNotificationId(notificationId);
            facilityCollection.SetIsInterim(isInterim);
        }
    }
}