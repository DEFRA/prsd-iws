namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IShipmentNumberHistotyRepository
    {
        Task<NumberOfShipmentsHistory> GetOriginalNumberOfShipments(Guid notificationId);
    }
}
