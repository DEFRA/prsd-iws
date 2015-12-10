namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementCompletedReceiptRepository : IImportMovementCompletedReceiptRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementCompletedReceiptRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportMovementCompletedReceipt> GetByMovementIdOrDefault(Guid movementId)
        {
            return await context.ImportMovementCompletedReceipts.SingleOrDefaultAsync(r => r.MovementId == movementId);
        }
    }
}
