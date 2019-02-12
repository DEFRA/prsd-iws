namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationOnlyNewShipmentsRule : IPrenotificationContentRule
    {
        private readonly IMovementRepository repo;

        public PrenotificationOnlyNewShipmentsRule(IMovementRepository repo)
        {
            this.repo = repo;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var shipments =
                (await repo.GetAllMovements(notificationId)).Join(
                        movements.Where(m => m.ShipmentNumber.HasValue).Select(m => m.ShipmentNumber.Value),
                        m => m.Number, s => s,
                        (m, s) => new { Movement = m, ShipmentNumber = s })
                    .GroupBy(x => x.ShipmentNumber)
                    .OrderBy(y => y.Key)
                    .Select(y => y.Key)
                    .ToList();

            var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

            var shipmentNumbers = string.Join(", ", shipments);
            var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.OnlyNewShipments), shipmentNumbers);

            return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.OnlyNewShipments, result, errorMessage);
        }
    }
}