namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationMovements;
    using Core.Shared;
    using Domain;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class ImportMovementsSummaryRepository : IImportMovementsSummaryRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportMovementsSummaryRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<Summary> GetById(Guid importNotificationId)
        {
            await authorization.EnsureAccessAsync(importNotificationId);

            var status =
                (await
                    context.ImportNotificationAssessments.SingleAsync(
                        n => n.NotificationApplicationId == importNotificationId)).Status;

            var notificationInfo = await context.ImportNotifications.SingleAsync(n => n.Id == importNotificationId);

            var totalMovements = await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).CountAsync();

            var shipment = await context.Shipments.Where(s => s.ImportNotificationId == importNotificationId).SingleAsync();

            var received = await TotalQuantityReceived(importNotificationId, shipment);

            var averageShipmentInfo = AveragePerShipment(importNotificationId, shipment);

            return new Summary
            {
                IntendedShipments = shipment.NumberOfShipments,
                UsedShipments = totalMovements,
                DisplayUnit = shipment.Quantity.Units,
                QuantityReceivedTotal = received.Quantity,
                QuantityRemainingTotal = shipment.Quantity.Quantity - received.Quantity,
                Id = importNotificationId,
                NotificationStatus = status,
                NotificationType = notificationInfo.NotificationType,
                NotificationNumber = notificationInfo.NotificationNumber,
                AverageTonnage = averageShipmentInfo.Quantity,
                AverageDataUnit = averageShipmentInfo.Units
            };
        }

        private async Task<ShipmentQuantity> TotalQuantityReceived(Guid importNotificationId, Shipment shipment)
        {
            await authorization.EnsureAccessAsync(importNotificationId);

            var movements = await context.ImportMovements.Where(m => m.NotificationId == importNotificationId).ToArrayAsync();

            var allMovementReceipts = new List<ImportMovementReceipt>();

            foreach (var movement in movements)
            {
                var movementReceipts = await context.ImportMovementReceipts.Where(mr => mr.MovementId == movement.Id).ToListAsync();
                allMovementReceipts = allMovementReceipts.Union(movementReceipts).ToList();
            }

            if (!allMovementReceipts.Any())
            {
                return new ShipmentQuantity(0, shipment == null ? ShipmentQuantityUnits.Tonnes : shipment.Quantity.Units);
            }
            
            var totalReceived = allMovementReceipts.Sum(m =>
                ShipmentQuantityUnitConverter.ConvertToTarget(
                    m.Unit,
                    shipment.Quantity.Units,
                    m.Quantity));

            return new ShipmentQuantity(totalReceived, shipment.Quantity.Units);
        }

        /// <summary>
        /// 1 ton = 1000 litres
        /// 1 ton = 1 cubic metre
        /// </summary>
        /// <param name="importNotificationId"></param>
        /// <returns></returns>
        private ShipmentQuantity AveragePerShipment(Guid importNotificationId, Shipment shipment)
        {            
            decimal shipmentQuantity;

            if (shipment.Quantity.Units == ShipmentQuantityUnits.Kilograms)
            {
                shipmentQuantity = ShipmentQuantityUnitConverter.ConvertToTarget(
                       shipment.Quantity.Units,
                       ShipmentQuantityUnits.Tonnes,
                       shipment.Quantity.Quantity);
            }
            else if (shipment.Quantity.Units == ShipmentQuantityUnits.Litres)
            {
                shipmentQuantity = shipment.Quantity.Quantity / 1000m;
            }
            else
            {
                shipmentQuantity = shipment.Quantity.Quantity;
            }

            return new ShipmentQuantity(Decimal.Divide(shipmentQuantity, shipment.NumberOfShipments), ShipmentQuantityUnits.Tonnes);
        }
    }
}
