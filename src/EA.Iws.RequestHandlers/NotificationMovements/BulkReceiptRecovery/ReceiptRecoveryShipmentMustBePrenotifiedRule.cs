﻿namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.Movement;

    public class ReceiptRecoveryShipmentMustBePrenotifiedRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository repo;

        public ReceiptRecoveryShipmentMustBePrenotifiedRule(IMovementRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var actualMovements = await repo.GetAllMovements(notificationId);

            List<int> shipments = new List<int>();
            MessageLevel result = MessageLevel.Success;

            foreach (var movement in movements.Where(p => p.ReceivedDate.HasValue && p.RecoveredDisposedDate.HasValue))
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement.Status == MovementStatus.Captured)
                {
                    if (actualMovement.Date.Date < DateTime.UtcNow.Date)
                    {
                        result = MessageLevel.Error;
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }           
                else if (actualMovement.Status == MovementStatus.Submitted)
                {
                    if (actualMovement.Date.Date > DateTime.UtcNow.Date)
                    {
                        result = MessageLevel.Error;
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }
                else
                {
                    result = MessageLevel.Error;
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var shipmentNumbers = string.Join(", ", shipments);
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.ReceivedRecoveredValidation), shipmentNumbers);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.ReceivedRecoveredValidation, result, errorMessage);
        }
    }
}