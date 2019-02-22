namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;

    public class ReceiptRecoveryShipmentMustBePrenotifiedRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository movementRepo;
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryShipmentMustBePrenotifiedRule(IMovementRepository movementRepo, INotificationApplicationRepository notificationRepo)
        {
            this.movementRepo = movementRepo;
            this.notificationRepo = notificationRepo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var actualMovements = (await movementRepo.GetAllMovements(notificationId)).ToList();
            var notification = await notificationRepo.GetById(notificationId);
            var shipments = new List<int>();

            var validMovements =
                movements.Where(
                    p =>
                        !p.MissingReceivedDate && !p.MissingRecoveredDisposedDate &&
                        p.ReceivedDate.HasValue && p.RecoveredDisposedDate.HasValue);

            foreach (var movement in validMovements)
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement == null)
                {
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    continue;
                }

                //Exclude these statuses as these will be picked up by Already Received and Already Recovered rules.
                if (actualMovement.Status != MovementStatus.Received &&
                    actualMovement.Status != MovementStatus.Completed)
                {
                    if (actualMovement.Status == MovementStatus.Captured)
                    {
                        if (actualMovement.Date.Date < SystemTime.UtcNow.Date)
                        {
                            shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                        }
                    }
                    else if (actualMovement.Status == MovementStatus.Submitted)
                    {
                        if (actualMovement.Date.Date > SystemTime.UtcNow.Date)
                        {
                            shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                        }
                    }
                    else
                    {
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }
            }

            var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
            var shipmentNumbers = string.Join(", ", shipments.Distinct());

            var type = notification.NotificationType == Core.Shared.NotificationType.Disposal ? "disposed" : "recovered";
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.ReceivedRecoveredValidation), shipmentNumbers, type);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.ReceivedRecoveredValidation, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
        }
    }
}