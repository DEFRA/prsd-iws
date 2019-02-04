namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentDuplicateShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
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
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.DuplicateShipmentNumber), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.DuplicateShipmentNumber, result, errorMessage);
            });
        }
    }
}
