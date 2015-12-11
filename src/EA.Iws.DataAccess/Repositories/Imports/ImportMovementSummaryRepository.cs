namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;

    internal class ImportMovementSummaryRepository : IImportMovementSummaryRepository
    {
        private readonly ImportNotificationContext context;

        public ImportMovementSummaryRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportMovementSummary> Get(Guid movementId)
        {
            var query = from movement in context.ImportMovements
                where movement.Id == movementId
                from notification 
                    in context.ImportNotifications
                        .Where(n => n.Id == movement.NotificationId)
                from receipt
                    in context.ImportMovementReceipts
                        .Where(x => x.MovementId == movementId).DefaultIfEmpty()
                from rejection
                    in context.ImportMovementRejections
                        .Where(x => x.MovementId == movementId).DefaultIfEmpty()
                from completedReceipt
                    in context.ImportMovementCompletedReceipts
                        .Where(x => x.MovementId == movementId).DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    Movement = movement,
                    Receipt = receipt,
                    Rejection = rejection,
                    CompletedReceipt = completedReceipt
                };

            var data = await query.SingleAsync();

            return new ImportMovementSummary(data.Movement,
                data.Receipt,
                data.Rejection,
                data.CompletedReceipt,
                data.Notification.NotificationType,
                data.Notification.NotificationNumber);
        }
    }
}
