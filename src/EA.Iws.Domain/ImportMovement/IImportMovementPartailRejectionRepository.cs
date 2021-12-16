namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementPartailRejectionRepository
    {
        Task<ImportMovementPartialRejection> GetByMovementId(Guid movementId);

        Task<ImportMovementPartialRejection> Get(Guid id);

        Task<ImportMovementPartialRejection> GetByMovementIdOrDefault(Guid movementId);

        void Add(ImportMovementPartialRejection movementRejection);
    }
}
