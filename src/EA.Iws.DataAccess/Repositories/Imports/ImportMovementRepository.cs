namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
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

        public async Task<IEnumerable<ImportMovement>> GetPrenotifiedForNotification(Guid importNotificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(importNotificationId);
            return await context.ImportMovements.Where(m => m.NotificationId == importNotificationId)
                .Where(m => !context.ImportMovementReceipts.Any(r => r.MovementId == m.Id))
                .Where(m => !context.ImportMovementCompletedReceipts.Any(r => r.MovementId == m.Id))
                .ToArrayAsync();
        }

        public void Add(ImportMovement movement)
        {
            context.ImportMovements.Add(movement);
        }

        public async Task<int> GetLatestMovementNumber(Guid importNotificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(importNotificationId);

            var movement = await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).OrderByDescending(m => m.ActualShipmentDate).ThenByDescending(m => m.Number).FirstOrDefaultAsync();

            return movement == null ? 0 : movement.Number;
        }

        public async Task<bool> DeleteById(Guid movementId)
        {
            var rowsAffected = await context.Database.ExecuteSqlCommandAsync(
                @"DELETE from [ImportNotification].[MovementOperationReceipt] WHERE MovementId = @movementId
                  DELETE from [ImportNotification].[MovementReceipt] WHERE MovementId = @movementId
                  DELETE from [ImportNotification].[MovementRejection] WHERE MovementId = @movementId
                  DELETE from [ImportNotification].[Movement] WHERE Id = @movementId",
                new SqlParameter("@movementId", movementId));

            return rowsAffected > 0;
        }
    }
}