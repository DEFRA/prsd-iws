namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class ReceiptRecoveryNotificationNumberRule : IReceiptRecoveryContentRule
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public ReceiptRecoveryNotificationNumberRule(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var notificationNumber = await notificationApplicationRepository.GetNumber(notificationId);
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(m => m.ShipmentNumber.HasValue && m.NotificationNumber != notificationNumber)
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                
                var shipmentNumbers = string.Join(", ", shipments.Distinct());
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.WrongNotificationNumber), shipmentNumbers, notificationNumber);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.WrongNotificationNumber, result, errorMessage);
            });
        }
    }
}
