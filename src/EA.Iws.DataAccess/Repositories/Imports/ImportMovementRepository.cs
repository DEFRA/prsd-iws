namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementRepository : IImportMovementRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementRepository(ImportNotificationContext context)
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

        public async Task<IEnumerable<ImportMovement>> GetForNotification(Guid importNotificationId)
        {
            return await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).ToArrayAsync();
        }

        public void Add(ImportMovement movement)
        {
            context.ImportMovements.Add(movement);
        }
    }
}