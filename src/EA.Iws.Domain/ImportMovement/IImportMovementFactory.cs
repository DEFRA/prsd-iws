namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementFactory
    {
        Task<ImportMovement> Create(Guid notificationId, int number, DateTime actualShipmentDate);
    }
}
