namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementRejectionRepository : IImportMovementRejectionRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementRejectionRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportMovementRejection> GetByMovementIdOrDefault(Guid movementId)
        {
            return await context.ImportMovementRejections.SingleOrDefaultAsync(r => r.MovementId == movementId);
        }

        public void Add(ImportMovementRejection rejection)
        {
            context.ImportMovementRejections.Add(rejection);
        }
    }
}
