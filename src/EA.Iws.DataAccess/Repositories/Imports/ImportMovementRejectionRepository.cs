namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementRejectionRepository : IImportMovementRejectionRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportMovementAuthorization authorization;

        public ImportMovementRejectionRepository(ImportNotificationContext context, IImportMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportMovementRejection> GetByMovementIdOrDefault(Guid movementId)
        {
            var rejection = await context.ImportMovementRejections.SingleOrDefaultAsync(r => r.MovementId == movementId);
            if (rejection != null)
            {
                await authorization.EnsureAccessAsync(movementId);
            }
            return rejection;
        }

        public void Add(ImportMovementRejection rejection)
        {
            context.ImportMovementRejections.Add(rejection);
        }
    }
}
