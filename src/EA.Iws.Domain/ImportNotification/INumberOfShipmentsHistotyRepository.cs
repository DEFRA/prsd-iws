namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface INumberOfShipmentsHistotyRepository
    {
        Task<NumberOfShipmentsHistory> GetOriginalNumberOfShipments(Guid notificationId);

        Task<int> GetCurrentNumberOfShipments(Guid notificationId);

        Task<int> GetLargestNumberOfShipments(Guid notificationId);
    }
}
