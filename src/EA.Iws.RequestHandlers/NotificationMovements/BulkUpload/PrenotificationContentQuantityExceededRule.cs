namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;

    public class PrenotificationContentQuantityExceededRule : IBulkMovementPrenotificationContentRule
    {
        private readonly INotificationMovementsSummaryRepository repo;

        public PrenotificationContentQuantityExceededRule(INotificationMovementsSummaryRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            var movementSummary = await this.repo.GetById(notificationId);

            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;

                decimal remainingQuantity = movementSummary.QuantityRemaining;
                decimal newQuantityCount = 0;
                int errorShipmentNumber = 0;

                foreach (var movement in movements.OrderBy(p => p.ShipmentNumber))
                {
                    newQuantityCount = newQuantityCount + movement.Quantity.GetValueOrDefault();

                    if (newQuantityCount > remainingQuantity)
                    {
                        missingDataResult = MessageLevel.Error;
                        errorShipmentNumber = movement.ShipmentNumber.GetValueOrDefault();
                        break;
                    }
                }

                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.QuantityExceeded), errorShipmentNumber);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.QuantityExceeded, missingDataResult, errorMessage);
            });
        }
    }
}