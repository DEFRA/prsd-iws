namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class ReceiptRecoveryRecoveryDateFormatRule : IReceiptRecoveryContentRule
    {
        private readonly INotificationApplicationRepository notificationRepo;

        public ReceiptRecoveryRecoveryDateFormatRule(INotificationApplicationRepository notificationRepo)
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

                foreach (var movement in movements)
                {
                    if (!movement.MissingRecoveredDisposedDate && movement.RecoveredDisposedDate == null)
                    {
                        result = MessageLevel.Error;
                        shipments.Add(movement.ShipmentNumber.GetValueOrDefault());
                    }
                }

                var shipmentNumbers = string.Join(", ", shipments.Distinct());
                string type = notification.NotificationType == Core.Shared.NotificationType.Disposal ? "disposal" : "recovery";
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveryDateFormat), shipmentNumbers, type);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.RecoveryDateFormat, result, errorMessage, shipments.DefaultIfEmpty(0).Min());
            });
        }
    }
}