namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;

    public class ReceiptRecoveryQuantityPrecisionLitresKilogramsRule : IReceiptRecoveryContentRule
    {
        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(
            List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                // This rule only works while the precision for Kilograms and Litres is the same
                var precision = ShipmentQuantityUnitsMetadata.Precision[ShipmentQuantityUnits.Kilograms];

                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                m.Quantity.HasValue && m.Unit.HasValue &&
                                (m.Unit.Value == ShipmentQuantityUnits.Litres ||
                                m.Unit.Value == ShipmentQuantityUnits.Kilograms) &&
                                decimal.Round(m.Quantity.Value, ShipmentQuantityUnitsMetadata.Precision[m.Unit.Value]) !=
                                m.Quantity.Value)
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments.Distinct());
                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.QuantityPrecision),
                        shipmentNumbers, precision);

                return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.QuantityPrecision,
                    result, errorMessage);
            });
        }
    }
}
