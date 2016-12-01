namespace EA.Iws.Domain.NotificationApplication.Shipment
{
    using System;
    using System.Threading.Tasks;

    public interface INumberOfShipmentsHistotyRepository
    {
        Task<NumberOfShipmentsHistory> GetOriginalNumberOfShipments(Guid notificationId);

        Task<int> GetCurrentNumberOfShipments(Guid notificationId);

        Task<int> GetLargestNumberOfShipments(Guid notificationId);
        
        void Add(NumberOfShipmentsHistory history);
    }
}
