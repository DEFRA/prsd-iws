namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Prsd.Core;

    public class PrenotificationContentDateHistoricRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var historicDateResult = MessageLevel.Success;
                var historicDateShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (movement.ShipmentNumber.HasValue && 
                    movement.ActualDateOfShipment.HasValue && 
                    movement.ActualDateOfShipment.Value < SystemTime.UtcNow)
                    {
                        historicDateResult = MessageLevel.Error;
                        historicDateShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", historicDateShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.HistoricDate), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.HistoricDate, historicDateResult, errorMessage);
            });
        }
    }
}
