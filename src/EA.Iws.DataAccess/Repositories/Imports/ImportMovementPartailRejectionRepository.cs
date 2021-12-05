namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using EA.Iws.Domain.ImportMovement;
    using EA.Iws.Domain.Security;

    internal class ImportMovementPartailRejectionRepository : IImportMovementPartailRejectionRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportMovementAuthorization authorization;

        public ImportMovementPartailRejectionRepository(ImportNotificationContext context, IImportMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportMovementPartialRejection> GetByMovementId(Guid movementId)
        {
            return await context.ImportMovementPartialRejections.SingleAsync(r => r.MovementId == movementId);
        }

        public async Task<ImportMovementPartialRejection> Get(Guid id)
        {
            return await context.ImportMovementPartialRejections.SingleAsync(r => r.Id == id);
        }

        public async Task<ImportMovementPartialRejection> GetByMovementIdOrDefault(Guid movementId)
        {
            var rejection = await context.ImportMovementPartialRejections.SingleOrDefaultAsync(r => r.MovementId == movementId);
            if (rejection != null)
            {
                await authorization.EnsureAccessAsync(movementId);
            }
            return rejection;
        }

        public void Add(ImportMovementPartialRejection rejection)
        {
            context.ImportMovementPartialRejections.Add(rejection);
        }
    }
}
