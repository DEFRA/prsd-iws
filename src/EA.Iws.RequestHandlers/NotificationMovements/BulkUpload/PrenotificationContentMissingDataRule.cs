namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingDataRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (var movement in movements)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (movement.ShipmentNumber.HasValue && 
                        (string.IsNullOrEmpty(movement.NotificationNumber) || 
                        !movement.ActualDateOfShipment.HasValue || 
                        !movement.PackagingTypes.Any() || 
                        !movement.Quantity.HasValue || 
                        !movement.Unit.HasValue))
                    {
                        missingDataResult = MessageLevel.Error;
                        missingDataShipmentNumbers.Add(movement.ShipmentNumber.ToString());
                    }
                }

                var shipmentNumbers = string.Join(", ", missingDataShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.MissingData), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, missingDataResult, errorMessage);
            });
        }
    }
}
