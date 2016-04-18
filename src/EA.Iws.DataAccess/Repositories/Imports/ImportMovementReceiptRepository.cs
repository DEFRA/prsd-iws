namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementReceiptRepository : IImportMovementReceiptRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementReceiptRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public void Add(ImportMovementReceipt receipt)
        {
            context.ImportMovementReceipts.Add(receipt);
        }

        public async Task<ImportMovementReceipt> GetByMovementIdOrDefault(Guid movementId)
        {
            return await context.ImportMovementReceipts.SingleOrDefaultAsync(r => r.MovementId == movementId);
        }
    }
}
