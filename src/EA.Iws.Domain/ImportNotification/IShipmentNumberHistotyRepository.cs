namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IShipmentNumberHistotyRepository
    {
        Task<ShipmentNumberHistory> GetOriginalNumberOfShipments(Guid notificationId);
    }
}
