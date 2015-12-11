namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IReceiveImportMovement
    {
        Task<ImportMovementReceipt> Receive(Guid movementId, ShipmentQuantity quantity, DateTimeOffset date);
    }
}