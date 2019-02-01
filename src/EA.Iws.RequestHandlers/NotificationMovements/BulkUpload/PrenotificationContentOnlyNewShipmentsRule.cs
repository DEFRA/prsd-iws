namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationContentOnlyNewShipmentsRule : IBulkMovementPrenotificationContentRule
    {
        private readonly IMovementRepository repo;

        public PrenotificationContentOnlyNewShipmentsRule(IMovementRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var result = MessageLevel.Success;

            var existingShipments =
                (await repo.GetAllMovements(notificationId)).Join(
                        movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value),
                        m => m.Number, s => s,
                        (m, s) => new { Movement = m, ShipmentNumber = s })
                    .OrderBy(x => x.ShipmentNumber)
                    .Select(x => x.ShipmentNumber)
                    .ToList();

            if (existingShipments.Any())
            {
                result = MessageLevel.Error;
            }

            var shipmentNumbers = string.Join(", ", existingShipments);

            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.OnlyNewShipments), shipmentNumbers);

            return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.OnlyNewShipments, result, errorMessage);
        }
    }
}