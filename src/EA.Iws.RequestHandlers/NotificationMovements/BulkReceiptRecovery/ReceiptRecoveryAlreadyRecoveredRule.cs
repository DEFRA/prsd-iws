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

    public class ReceiptRecoveryAlreadyRecoveredRule : IReceiptRecoveryContentRule
    {
        private readonly IMovementRepository movementRepo;
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryAlreadyRecoveredRule(IMovementRepository movementRepo, INotificationApplicationRepository notificationRepo)
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
                movements.Where(p => !p.MissingRecoveredDisposedDate && p.RecoveredDisposedDate.HasValue);

            foreach (var movement in validMovements)
            {
                var actualMovement = actualMovements.FirstOrDefault(p => p.Number == movement.ShipmentNumber);

                if (actualMovement != null && actualMovement.Status == MovementStatus.Completed)
                {
                    shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                }
            }

            var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
            var shipmentNumbers = string.Join(", ", shipments.Distinct());

            var type = notification.NotificationType == NotificationType.Disposal ? "disposed" : "recovered";
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed), shipmentNumbers, type);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.AlreadyRecievedRecoveredDisposed, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
        }
    }
}