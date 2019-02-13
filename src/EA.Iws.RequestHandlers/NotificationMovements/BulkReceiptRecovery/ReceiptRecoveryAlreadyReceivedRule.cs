namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;

    public class ReceiptRecoveryAlreadyReceivedRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository movementRepo;

        public ReceiptRecoveryAlreadyReceivedRule(IMovementRepository movementRepo)
        {
            this.movementRepo = movementRepo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var actualMovements = await movementRepo.GetAllMovements(notificationId);

            List<int> shipments = new List<int>();
            MessageLevel result = MessageLevel.Success;

            foreach (var movement in movements.Where(p => p.ReceivedDate.HasValue))
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement != null && (actualMovement.Status == MovementStatus.Received || actualMovement.Status == MovementStatus.Completed))
                {
                    result = MessageLevel.Error;
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var shipmentNumbers = string.Join(", ", shipments.Distinct());

            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed), shipmentNumbers, "received");

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed, result, errorMessage, shipments.Min());
        }
    }
}