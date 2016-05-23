namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementReceiptRepository : IImportMovementReceiptRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportMovementAuthorization authorization;

        public ImportMovementReceiptRepository(ImportNotificationContext context, IImportMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(ImportMovementReceipt receipt)
        {
            context.ImportMovementReceipts.Add(receipt);
        }

        public async Task<ImportMovementReceipt> GetByMovementIdOrDefault(Guid movementId)
        {
            var receipt = await context.ImportMovementReceipts.SingleOrDefaultAsync(r => r.MovementId == movementId);
            if (receipt != null)
            {
                await authorization.EnsureAccessAsync(movementId);
            }
            return receipt;
        }
    }
}
