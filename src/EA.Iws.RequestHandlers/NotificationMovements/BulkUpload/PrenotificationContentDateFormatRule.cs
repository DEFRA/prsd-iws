namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentDateFormatRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var invalidFormatResult = MessageLevel.Success;
                var invalidFormatShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (movement.ShipmentNumber.HasValue && 
                    !movement.MissingDateOfShipment && 
                    !movement.ActualDateOfShipment.HasValue)
                    {
                        invalidFormatResult = MessageLevel.Error;
                        invalidFormatShipmentNumbers.Add(movement.ShipmentNumber.Value.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", invalidFormatShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.InvalidDateFormat), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidDateFormat, invalidFormatResult, errorMessage);
            });
        }
    }
}
