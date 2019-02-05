namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Core.Shared;
    using Domain.NotificationApplication.Shipment;

    public class PrenotificationContentQuantityUnitRule : IPrenotificationContentRule
    {
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public PrenotificationContentQuantityUnitRule(IShipmentInfoRepository shipmentInfoRepository)
        {
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(
            List<PrenotificationMovement> movements, Guid notificationId)
        {
            var shipment = await shipmentInfoRepository.GetByNotificationId(notificationId);
            var units = shipment == null ? default(ShipmentQuantityUnits) : shipment.Units;
            var availableUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(units).ToList();

            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                (!m.Unit.HasValue ||
                                availableUnits.All(u => u != m.Unit.Value)))
                        .Select(m => m.ShipmentNumber.Value)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage =
                    string.Format(
                        Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.QuantityUnit),
                        shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.QuantityUnit,
                    result, errorMessage);
            });
        }
    }
}
