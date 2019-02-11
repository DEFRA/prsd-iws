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

    public class ReceiptRecoveryRecoveryOnlyRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository movementRepo;
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryRecoveryOnlyRule(IMovementRepository movementRepo, INotificationApplicationRepository notificationRepo)
        {
            this.movementRepo = movementRepo;
            this.notificationRepo = notificationRepo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var actualMovements = await movementRepo.GetAllMovements(notificationId);
            var notification = await notificationRepo.GetById(notificationId);

            List<int> shipments = new List<int>();
            MessageLevel result = MessageLevel.Success;

            foreach (var movement in movements.Where(p => !p.ReceivedDate.HasValue && p.RecoveredDisposedDate.HasValue && !p.MissingRecoveredDisposedDate))
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement.Status != MovementStatus.Received)
                {
                    result = MessageLevel.Error;
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var shipmentNumbers = string.Join(", ", shipments);
            string type = notification.NotificationType == Core.Shared.NotificationType.Disposal ? "disposed" : "recovered";
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveredValidation), shipmentNumbers, type);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.RecoveredValidation, result, errorMessage);
        }
    }
}