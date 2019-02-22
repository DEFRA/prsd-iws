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
    using Prsd.Core;

    public class ReceiptRecoveryReceiptOnlyRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository repo;

        public ReceiptRecoveryReceiptOnlyRule(IMovementRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var actualMovements = (await repo.GetAllMovements(notificationId)).ToList();
            var shipments = new List<int>();

            var validMovements =
                movements.Where(
                    p =>
                        !p.MissingReceivedDate && p.MissingRecoveredDisposedDate &&
                        p.ReceivedDate.HasValue);

            foreach (var movement in validMovements)
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement == null)
                {
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    continue;
                }

                if (actualMovement.Status != MovementStatus.Received)
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
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.PrenotifiedShipment), shipmentNumbers);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.PrenotifiedShipment, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
        }
    }
} 