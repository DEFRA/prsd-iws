namespace EA.Iws.Domain.ImportMovement
{
    using EA.Iws.Core.Shared;
    using System;
    using System.Threading.Tasks;

    public interface IRejectImportMovement
    {
        Task<ImportMovementRejection> Reject(Guid importMovementId, DateTime date, string reason, decimal? quantity, ShipmentQuantityUnits? unit);
    }
}
