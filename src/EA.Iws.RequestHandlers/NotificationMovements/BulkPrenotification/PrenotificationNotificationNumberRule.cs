namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.NotificationApplication;

    public class PrenotificationNotificationNumberRule : IPrenotificationContentRule
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public PrenotificationNotificationNumberRule(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var notificationNumber = await notificationApplicationRepository.GetNumber(notificationId);
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(m => m.ShipmentNumber.HasValue && m.NotificationNumber != notificationNumber)
                        .GroupBy(x => x.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.WrongNotificationNumber), shipmentNumbers, notificationNumber, minShipment);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.WrongNotificationNumber, result, errorMessage, minShipment);
            });
        }
    }
}
