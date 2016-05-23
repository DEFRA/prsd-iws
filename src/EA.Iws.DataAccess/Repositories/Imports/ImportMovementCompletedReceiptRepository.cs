namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementCompletedReceiptRepository : IImportMovementCompletedReceiptRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportMovementAuthorization authorization;

        public ImportMovementCompletedReceiptRepository(ImportNotificationContext context, IImportMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportMovementCompletedReceipt> GetByMovementIdOrDefault(Guid movementId)
        {
            var receipt = await context.ImportMovementCompletedReceipts.SingleOrDefaultAsync(r => r.MovementId == movementId);
            if (receipt != null)
            {
                await authorization.EnsureAccessAsync(movementId);
            }
            return receipt;
        }

        public void Add(ImportMovementCompletedReceipt completedReceipt)
        {
            context.ImportMovementCompletedReceipts.Add(completedReceipt);
        }
    }
}
