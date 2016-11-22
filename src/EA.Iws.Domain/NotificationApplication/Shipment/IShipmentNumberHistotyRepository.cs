namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using System.Threading.Tasks;
    using Core.Shipment;

    public interface IShipmentNumberHistotyRepository
    {
        Task<ShipmentNumberHistory> GetOriginalNumberOfShipments(Guid notificationId);
    }
}
