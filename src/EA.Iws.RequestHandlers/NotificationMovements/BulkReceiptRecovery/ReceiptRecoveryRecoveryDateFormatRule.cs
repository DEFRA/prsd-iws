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
                var shipments =
                    movements.Where(m => m.ShipmentNumber.HasValue && !m.MissingRecoveredDisposedDate && !m.RecoveredDisposedDate.HasValue)
                        .GroupBy(x => x.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                string type = notification.NotificationType == Core.Shared.NotificationType.Disposal ? "disposal" : "recovery";
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.RecoveryDateFormat), shipmentNumbers, type);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.RecoveryDateFormat, result, errorMessage, minShipment);
            });
        }
    }
}