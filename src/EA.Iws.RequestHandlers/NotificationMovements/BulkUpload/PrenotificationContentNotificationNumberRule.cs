namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class PrenotificationContentNotificationNumberRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationContentNotificationNumberRule(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var notificationNumber = await notificationApplicationRepository.GetNumber(notificationId);
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(m => m.ShipmentNumber.HasValue && m.NotificationNumber != notificationNumber)
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                
                var shipmentNumbers = string.Join(", ", result);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.WrongNotificationNumber), shipmentNumbers, notificationNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.WrongNotificationNumber, result, errorMessage);
            });
        }
    }
}
