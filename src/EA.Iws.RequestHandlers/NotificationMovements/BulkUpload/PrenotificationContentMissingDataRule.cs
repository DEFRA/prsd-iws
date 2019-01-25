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

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, missingDataResult, missingDataShipmentNumbers);
            });
        }
    }
}
