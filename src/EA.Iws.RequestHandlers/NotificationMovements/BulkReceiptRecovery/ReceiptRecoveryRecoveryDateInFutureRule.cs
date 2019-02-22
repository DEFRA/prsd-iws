namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.NotificationApplication;
    using Prsd.Core;

    public class ReceiptRecoveryRecoveryDateInFutureRule : IReceiptRecoveryContentRule
    {
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryRecoveryDateInFutureRule(INotificationApplicationRepository notificationRepo)
        {
            this.notificationRepo = notificationRepo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var notification = await notificationRepo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var shipments = new List<int>();

                foreach (
                    var movement in
                    movements.Where(p => !p.MissingRecoveredDisposedDate && p.RecoveredDisposedDate.HasValue))
                {
                    if (movement.RecoveredDisposedDate > SystemTime.UtcNow ||
                        movement.RecoveredDisposedDate < movement.ReceivedDate)
                    {
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var shipmentNumbers = string.Join(", ", shipments.Distinct());

                var type = notification.NotificationType == Core.Shared.NotificationType.Disposal ? "disposal" : "recovery";
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveryDateValidation), shipmentNumbers, type);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.RecoveryDateValidation, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
            });
        }
    }
}