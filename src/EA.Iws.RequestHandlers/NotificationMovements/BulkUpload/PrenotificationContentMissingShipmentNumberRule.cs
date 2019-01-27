namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingShipmentNumberResult = MessageLevel.Success;
                int missingShipmentNumberCount = 0;

                foreach (PrenotificationMovement shipment in shipments)
                {
                    if (!shipment.HasShipmentNumber)
                    {
                        missingShipmentNumberResult = MessageLevel.Error;
                        missingShipmentNumberCount++;
                    }
                }
                
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.MissingShipmentNumbers), missingShipmentNumberCount);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingShipmentNumbers, missingShipmentNumberResult, errorMessage);
            });
        }
    }
}
