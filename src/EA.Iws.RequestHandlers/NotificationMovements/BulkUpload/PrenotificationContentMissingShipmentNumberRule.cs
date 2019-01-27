namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingShipmentNumberResult = MessageLevel.Success;
                var missingShipmentNumberCount = 0;

                foreach (var movement in movements)
                {
                    if (!movement.ShipmentNumber.HasValue)
                    {
                        missingShipmentNumberResult = MessageLevel.Error;
                        missingShipmentNumberCount++;
                    }
                }

                var ruleResult =
                    new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingShipmentNumbers,
                        missingShipmentNumberResult, new List<string>())
                    {
                        ErroneousShipmentCount = missingShipmentNumberCount
                    };

                return ruleResult;
            });
        }
    }
}
