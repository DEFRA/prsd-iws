namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementRejectionRepository
    {
        Task<ImportMovementRejection> GetByMovementIdOrDefault(Guid movementId);
    }
}
