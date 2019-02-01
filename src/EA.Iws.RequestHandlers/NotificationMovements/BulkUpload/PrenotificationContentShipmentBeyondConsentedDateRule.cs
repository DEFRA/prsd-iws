namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.NotificationApplication.Shipment;

    public class PrenotificationContentShipmentBeyondConsentedDateRule : IBulkMovementPrenotificationContentRule
    {
        private readonly IShipmentInfoRepository repo;

        public PrenotificationContentShipmentBeyondConsentedDateRule(IShipmentInfoRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var shipmentInfo = await repo.GetByNotificationId(notificationId);

            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue && m.ActualDateOfShipment.HasValue &&
                                m.ActualDateOfShipment.Value > shipmentInfo.ShipmentPeriod.LastDate)
                                .OrderBy(m => m.ActualDateOfShipment.Value)
                        .Select(m => m.ShipmentNumber.GetValueOrDefault())
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.BeyondConsentWindow), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.BeyondConsentWindow, result, errorMessage);
            });
        }
    }
}