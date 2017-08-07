namespace EA.Iws.DataAccess.Repositories.Imports
{
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Domain.Security;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task<IEnumerable<ImportMovement>> GetRejectedMovements(Guid notificationId)
        {
            await notificationAuthorization.EnsureAccessAsync(notificationId);

            var movements = await context.ImportMovements.Where(m => m.NotificationId == notificationId).ToArrayAsync();
            var rejectedMovements = new List<ImportMovement>();

            foreach (var m in movements)
            {
                var rejection = await context.ImportMovementRejections.SingleOrDefaultAsync(r => r.MovementId == m.Id);
                if (rejection != null)
                {
                    rejectedMovements.Add(m);
                }
            }

            return rejectedMovements;
        }

        public async Task SetImportMovementReceiptAndRecoveryData(ImportMovementSummaryData data, Guid createdBy)
        {
            await context.Database.ExecuteSqlCommandAsync(@"[ImportNotification].[uspUpdateImportMovementData] 
                @NotificationId
                ,@MovementId
                ,@PrenotificationDate
                ,@ActualDate
                ,@ReceiptDate
                ,@Quantity
                ,@Unit
                ,@RejectiontDate
                ,@RejectionReason
                ,@StatsMarking
                ,@Comments
                ,@RecoveryDate
                ,@CreatedBy",
                new SqlParameter("@NotificationId", data.Data.NotificationId),
                new SqlParameter("@MovementId", data.MovementId),
                new SqlParameter("@PrenotificationDate", (object)data.Data.PreNotificationDate ?? DBNull.Value),
                new SqlParameter("@ActualDate", (object)data.Data.ActualDate ?? DBNull.Value),
                new SqlParameter("@ReceiptDate", (object)data.ReceiptData.ReceiptDate ?? DBNull.Value),
                new SqlParameter("@Quantity", (object)data.ReceiptData.ActualQuantity ?? DBNull.Value),
                new SqlParameter("@Unit", (object)data.ReceiptData.ReceiptUnits ?? DBNull.Value),
                new SqlParameter("@RejectiontDate", (object)data.RejectionDate ?? DBNull.Value),
                new SqlParameter("@RejectionReason", (object)data.ReceiptData.RejectionReason ?? DBNull.Value),
                new SqlParameter("@StatsMarking", (object)data.StatsMarking ?? DBNull.Value),
                new SqlParameter("@Comments", (object)data.Comments ?? DBNull.Value),
                 new SqlParameter("@RecoveryDate", (object)data.RecoveryData.OperationCompleteDate ?? DBNull.Value),
                new SqlParameter("@CreatedBy", createdBy));
        }
    }
}