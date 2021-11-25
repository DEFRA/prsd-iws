﻿namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementSummaryRepository : IImportMovementSummaryRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportMovementAuthorization authorization;

        public ImportMovementSummaryRepository(ImportNotificationContext context, IImportMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportMovementSummary> Get(Guid movementId)
        {
            await authorization.EnsureAccessAsync(movementId);

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
                from partialRejection
                    in context.ImportMovementPartialRejections
                        .Where(x => x.MovementId == movementId).DefaultIfEmpty()
                from completedReceipt
                    in context.ImportMovementCompletedReceipts
                        .Where(x => x.MovementId == movementId).DefaultIfEmpty()
                from shipment
                    in context.Shipments
                    .Where(x => x.ImportNotificationId == notification.Id).DefaultIfEmpty()
                select new
                {
                    Notification = notification,
                    Movement = movement,
                    Receipt = receipt,
                    Rejection = rejection,
                    CompletedReceipt = completedReceipt,
                    Shipment = shipment,
                    PartialRejection = partialRejection
                };

            var data = await query.SingleAsync();

            return new ImportMovementSummary(data.Movement,
                data.Receipt,
                data.Rejection,
                data.CompletedReceipt,
                data.Notification.NotificationType,
                data.Notification.NotificationNumber,
                data.Shipment.Quantity.Units,
                data.PartialRejection);
        }
    }
}
