namespace EA.Iws.RequestHandlers.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;

    public class ReceiptRecoveryRecoveryQuantityUnitRule : IReceiptRecoveryContentRule
    {
        private readonly IShipmentInfoRepository repo;

        public ReceiptRecoveryRecoveryQuantityUnitRule(IShipmentInfoRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>> GetResult(List<ReceiptRecoveryMovement> movements, Guid notificationId)
        {
            var shippingInfo = await repo.GetByNotificationId(notificationId);
            var units = shippingInfo == null ? default(ShipmentQuantityUnits) : shippingInfo.Units;
            var availableUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(units).ToList();

            var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                !m.MissingUnits &&
                                (!m.Unit.HasValue ||
                                availableUnits.All(u => u != m.Unit.Value)))
                        .GroupBy(x => x.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

            var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
            var minShipment = shipments.FirstOrDefault() ?? 0;

            var shipmentNumbers = string.Join(", ", shipments);
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(ReceiptRecoveryContentRules.QuantityUnit), shipmentNumbers);

            return new ReceiptRecoveryContentRuleResult<ReceiptRecoveryContentRules>(ReceiptRecoveryContentRules.QuantityUnit, result, errorMessage, minShipment);
        }
    }
}