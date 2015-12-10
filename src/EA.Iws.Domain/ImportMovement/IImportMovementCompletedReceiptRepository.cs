namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementCompletedReceiptRepository
    {
        Task<ImportMovementCompletedReceipt> GetByMovementIdOrDefault(Guid movementId);
    }
}
