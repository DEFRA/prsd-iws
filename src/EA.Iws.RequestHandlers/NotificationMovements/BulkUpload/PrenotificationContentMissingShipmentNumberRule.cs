namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingShipmentNumberRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments)
        {
            return await Task.Run(() =>
            {
                var missingShipmentNumberResult = MessageLevel.Success;
                int missingShipmentNumberCount = 0;

                foreach (PrenotificationMovement shipment in shipments)
                {
                    if (shipment.ShipmentNumber.Equals(string.Empty))
                    {
                        missingShipmentNumberResult = MessageLevel.Error;
                        missingShipmentNumberCount++;
                    }
                }

                // COULLM: missingShipmentNumberCount needs to be used to populate error message in ContentRuleResult 

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingShipmentNumbers, missingShipmentNumberResult, new List<string>());
            });
        }
    }
}
