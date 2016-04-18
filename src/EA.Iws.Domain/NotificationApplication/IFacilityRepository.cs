namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface IFacilityRepository
    {
        Task<FacilityCollection> GetByNotificationId(Guid notificationId);

        void Add(FacilityCollection facilityCollection);

        Task<FacilityCollection> GetByMovementId(Guid movementId);
    }
}