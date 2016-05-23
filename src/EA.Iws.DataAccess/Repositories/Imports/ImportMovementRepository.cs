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
        private readonly IImportMovementAuthorization authorization;
        private readonly IImportNotificationApplicationAuthorization notificationAuthorization;

        public ImportMovementRepository(ImportNotificationContext context, IImportMovementAuthorization authorization, IImportNotificationApplicationAuthorization notificationAuthorization)
        {
            this.context = context;
            this.authorization = authorization;
            this.notificationAuthorization = notificationAuthorization;
        }

        public async Task<ImportMovement> Get(Guid id)
        {
            await authorization.EnsureAccessAsync(id);
            return await context.ImportMovements.SingleAsync(m => m.Id == id);
        }

        public async Task<ImportMovement> GetByNumberOrDefault(Guid importNotificationId, int number)
        {
            var movement = await context.ImportMovements.SingleOrDefaultAsync(m => m.NotificationId == importNotificationId
                && m.Number == number);

            if (movement != null)
            {
                await authorization.EnsureAccessAsync(movement.Id);
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