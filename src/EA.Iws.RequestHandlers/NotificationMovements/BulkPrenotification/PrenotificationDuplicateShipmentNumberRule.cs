namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;

    public class PrenotificationDuplicateShipmentNumberRule : IPrenotificationContentRule
    {
        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var shipments = movements.Where(m => m.ShipmentNumber.HasValue)
                    .GroupBy(x => x.ShipmentNumber)
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.DuplicateShipmentNumber), shipmentNumbers);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.DuplicateShipmentNumber, result, errorMessage);
            });
        }
    }
}
