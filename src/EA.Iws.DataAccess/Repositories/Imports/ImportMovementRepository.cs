namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementRepository : IImportMovementRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization notificationAuthorization;

        public ImportMovementRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization notificationAuthorization)
        {
            this.context = context;
            this.notificationAuthorization = notificationAuthorization;
        }

        public async Task<ImportMovement> Get(Guid id)
        {
            var movement = await context.ImportMovements.SingleAsync(m => m.Id == id);
            await notificationAuthorization.EnsureAccessAsync(movement.NotificationId);
            return movement;
        }

        public async Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number)
        {
            var movement = await context.ImportMovements.SingleOrDefaultAsync(m => m.NotificationId == importNotificationId
                && m.Number == number);

            if (movement != null)
            {
                await notificationAuthorization.EnsureAccessAsync(movement.NotificationId);
            }

            return movement;
        }

        public async Task<IEnumerable<ImportMovement>> GetForNotification(Guid importNotificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(importNotificationId);
            return await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).ToArrayAsync();
        }

        public void Add(ImportMovement movement)
        {
            context.ImportMovements.Add(movement);
        }
    }
}