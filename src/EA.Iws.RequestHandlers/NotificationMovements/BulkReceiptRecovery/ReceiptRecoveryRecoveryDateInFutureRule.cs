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
                List<int> shipments = new List<int>();
                MessageLevel result = MessageLevel.Success;

                foreach (var movement in movements.Where(p => !p.MissingRecoveredDisposedDate))
                {
                    if (movement.RecoveredDisposedDate > DateTime.Now || movement.RecoveredDisposedDate < movement.ReceivedDate)
                    {
                        result = MessageLevel.Error;
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }

                var shipmentNumbers = string.Join(", ", shipments.Distinct());
                string type = notification.NotificationType == Core.Shared.NotificationType.Disposal ? "disposal" : "recovery";
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveryDateValidation), shipmentNumbers, type);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.RecoveryDateValidation, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
            });
        }
    }
}