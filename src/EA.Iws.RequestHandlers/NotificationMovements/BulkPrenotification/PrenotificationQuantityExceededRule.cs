namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationQuantityExceededRule : IPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository repo;

        public PrenotificationQuantityExceededRule(INotificationMovementsSummaryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var movementSummary = await this.repo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var result = MessageLevel.Success;

                var remainingQuantity = movementSummary.QuantityRemaining;
                var newQuantityCount = 0m;
                var errorShipmentNumber = 0;

                foreach (var movement in movements.OrderBy(p => p.ShipmentNumber))
                {
                    newQuantityCount = newQuantityCount + movement.Quantity.GetValueOrDefault();

                    if (newQuantityCount > remainingQuantity)
                    {
                        result = MessageLevel.Error;
                        errorShipmentNumber = movement.ShipmentNumber.GetValueOrDefault();
                        break;
                    }
                }

                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.QuantityExceeded), errorShipmentNumber);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.QuantityExceeded, result, errorMessage, errorShipmentNumber);
            });
        }
    }
}