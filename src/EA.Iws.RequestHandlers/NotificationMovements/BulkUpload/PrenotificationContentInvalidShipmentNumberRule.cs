namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationContentInvalidShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository repo;

        public PrenotificationContentInvalidShipmentNumberRule(INotificationMovementsSummaryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var movementSummary = await this.repo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue &&
                                m.ShipmentNumber.Value > movementSummary.IntendedTotalShipments)
                        .Select(m => m.ShipmentNumber.Value)
                        .OrderBy(m => m)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.InvalidShipmentNumber), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidShipmentNumber, result, errorMessage);
            });
        }
    }
}