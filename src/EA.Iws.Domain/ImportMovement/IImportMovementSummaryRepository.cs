namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementSummaryRepository
    {
        Task<ImportMovementSummary> Get(Guid movementId);
    }
}
