namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementReceiptRepository
    {
        void Add(ImportMovementReceipt receipt);

        Task<ImportMovementReceipt> GetByMovementIdOrDefault(Guid movementId);
    }
}
