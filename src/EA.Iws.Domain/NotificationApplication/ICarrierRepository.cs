namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface ICarrierRepository
    {
        Task<CarrierCollection> GetByNotificationId(Guid notificationId);

        Task<CarrierCollection> GetByMovementId(Guid movementId);

        void Add(CarrierCollection carrierCollection);
    }
}