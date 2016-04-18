namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using System.Threading.Tasks;
    using Core.IntendedShipments;

    public interface IShipmentInfoRepository
    {
        Task<ShipmentInfo> GetByNotificationId(Guid notificationId);

        Task<IntendedShipmentData> GetIntendedShipmentDataByNotificationId(Guid notificationId);
    }
}
