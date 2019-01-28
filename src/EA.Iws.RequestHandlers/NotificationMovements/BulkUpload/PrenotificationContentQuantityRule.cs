namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Core.Shared;

    public class PrenotificationContentQuantityRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(
            List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var result = MessageLevel.Success;
                var failedShipments = new List<string>();

                foreach (var movement in movements)
                {
                    var quantity = movement.Quantity;
                    var unit = movement.Unit;

                    if (movement.ShipmentNumber.HasValue && quantity.HasValue && unit.HasValue &&
                        decimal.Round(quantity.Value, ShipmentQuantityUnitsMetadata.Precision[unit.Value]) != quantity)
                    {
                        result = MessageLevel.Error;
                        failedShipments.Add(movement.ShipmentNumber.Value.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", failedShipments);
                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.QuantityPrecision),
                        shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.QuantityPrecision,
                    result, errorMessage);
            });
        }
    }
}
