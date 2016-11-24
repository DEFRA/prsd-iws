namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using System.Threading.Tasks;

    public interface INumberOfShipmentsHistotyRepository
    {
        Task<NumberOfShipmentsHistory> GetOriginalNumberOfShipments(Guid notificationId);
    }
}
