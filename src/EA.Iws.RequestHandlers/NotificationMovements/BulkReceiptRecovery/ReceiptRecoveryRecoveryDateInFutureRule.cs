namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;

    public class ReceiptRecoveryRecoveryDateInFutureRule : IReceiptRecoveryContentRule
    {
        private readonly INotificationApplicationRepository notificationRepo;
        private readonly IMovementRepository movementRepo;

        public ReceiptRecoveryRecoveryDateInFutureRule(INotificationApplicationRepository notificationRepo,
            IMovementRepository movementRepo)
        {
            this.notificationRepo = notificationRepo;
            this.movementRepo = movementRepo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(
            List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var notification = await notificationRepo.GetById(notificationId);
            var actualMovements = (await movementRepo.GetAllMovements(notificationId)).ToList();

            var shipments = new List<int>();

            foreach (
                var movement in
                movements.Where(p => !p.MissingRecoveredDisposedDate && p.RecoveredDisposedDate.HasValue))
            {
                var receivedDate = GetReceivedDate(movement, actualMovements);

                if (movement.RecoveredDisposedDate > SystemTime.UtcNow ||
                    movement.RecoveredDisposedDate < receivedDate)
                {
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
            var shipmentNumbers = string.Join(", ", shipments.Distinct());

            var type = notification.NotificationType == Core.Shared.NotificationType.Disposal
                ? "disposal"
                : "recovery";
            var errorMessage =
                string.Format(
                    Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveryDateValidation),
                    shipmentNumbers, type);

            return
                new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(
                    ReceiptRecoveryContentRules.RecoveryDateValidation, result, errorMessage,
                    shipments.DefaultIfEmpty(0).Min());
        }

        private static DateTime? GetReceivedDate(ReceiptRecoveryMovement movement, IEnumerable<Movement> acualMovements)
        {
            DateTime? receivedDate = null;

            if (!movement.MissingReceivedDate)
            {
                return movement.ReceivedDate;
            }

            var actualMovement = acualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

            if (actualMovement != null)
            {
                receivedDate = actualMovement.Receipt.Date;
            }

            return receivedDate;
        }
    }
}