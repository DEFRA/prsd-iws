namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IFacilityRepository
    {
        Task<FacilityCollection> GetByNotificationId(Guid notificationId);

        void Add(FacilityCollection facilityCollection);
    }
}