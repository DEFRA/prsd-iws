namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingDataRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (PrenotificationMovement shipment in shipments)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (shipment.HasShipmentNumber)
                    {
                        if (shipment.ActualDateOfShipment.Equals(string.Empty) ||
                        shipment.NotificationNumber.Equals(string.Empty) ||
                        shipment.PackagingType.Equals(string.Empty) ||
                        shipment.Quantity.Equals(string.Empty) ||
                        shipment.Unit.Equals(string.Empty))
                        {
                            missingDataResult = MessageLevel.Error;
                            missingDataShipmentNumbers.Add(shipment.ShipmentNumber);
                        }
                    }
                }

                var shipmentNumbers = string.Join(", ", missingDataShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.MissingData), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, missingDataResult, errorMessage);
            });
        }
    }
}
