namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface ICompleteImportMovement
    {
        Task<ImportMovementCompletedReceipt> Complete(Guid movementId, DateTime date);
    }
}
