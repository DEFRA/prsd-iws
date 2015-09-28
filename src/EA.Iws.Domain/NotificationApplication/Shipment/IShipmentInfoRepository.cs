namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using System.Threading.Tasks;

    public interface IShipmentInfoRepository
    {
        Task<ShipmentInfo> GetByNotificationId(Guid notificationId);
    }
}
