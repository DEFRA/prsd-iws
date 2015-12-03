namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementRepository : IImportMovementRepository
    {
        private readonly IwsContext context;

        public ImportMovementRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<ImportMovement> Get(Guid id)
        {
            return await context.ImportMovements.SingleAsync(m => m.Id == id);
        }

        public async Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number)
        {
            return await context.ImportMovements.SingleOrDefaultAsync(m => m.NotificationId == importNotificationId
                                                                           && m.Number == number);
        }
    }
}