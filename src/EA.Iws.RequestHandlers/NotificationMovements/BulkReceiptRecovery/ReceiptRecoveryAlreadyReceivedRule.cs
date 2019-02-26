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

    public class ReceiptRecoveryAlreadyReceivedRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository movementRepo;

        public ReceiptRecoveryAlreadyReceivedRule(IMovementRepository movementRepo)
        {
            this.movementRepo = movementRepo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var actualMovements = (await movementRepo.GetAllMovements(notificationId)).ToList();
            var shipments = new List<int>();

            var validMovements =
                movements.Where(p => !p.MissingReceivedDate && p.ReceivedDate.HasValue)
                    .OrderBy(p => p.ShipmentNumber)
                    .ToList();

            foreach (var movement in validMovements)
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement != null && actualMovement.Status == MovementStatus.Received)
                {
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

            var shipmentNumbers = string.Join(", ", shipments.Distinct());

            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed), shipmentNumbers, "received");

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
        }
    }
}