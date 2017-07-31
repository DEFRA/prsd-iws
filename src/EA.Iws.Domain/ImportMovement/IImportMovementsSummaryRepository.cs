namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotificationMovements;
    using Core.Shared;

    public interface IImportMovementsSummaryRepository
    {
        Task<Summary> GetById(Guid importNotificationId);

        Task<ShipmentQuantity> AveragePerShipment(Guid importNotificationId);
    }
}
