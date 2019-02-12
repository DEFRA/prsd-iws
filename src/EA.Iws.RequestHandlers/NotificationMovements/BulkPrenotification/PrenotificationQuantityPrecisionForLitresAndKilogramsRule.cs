namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Core.Shared;

    public class PrenotificationQuantityPrecisionForLitresAndKilogramsRule : IPrenotificationContentRule
    {
        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(
            List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                // This rule only works while the precision for Litres and Kilograms is the same
                var precision = ShipmentQuantityUnitsMetadata.Precision[ShipmentQuantityUnits.Litres];

                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                m.Quantity.HasValue && m.Unit.HasValue &&
                                (m.Unit.Value == ShipmentQuantityUnits.Litres ||
                                m.Unit.Value == ShipmentQuantityUnits.Kilograms) &&
                                decimal.Round(m.Quantity.Value, ShipmentQuantityUnitsMetadata.Precision[m.Unit.Value]) !=
                                m.Quantity.Value)
                        .GroupBy(x => x.ShipmentNumber)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.QuantityPrecision),
                        shipmentNumbers, precision);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.QuantityPrecision,
                    result, errorMessage);
            });
        }
    }
}