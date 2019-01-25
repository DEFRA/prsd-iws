namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentMissingDataRule : IBulkMovementPrenotificationContentRule
    {
        public PrenotificationContentMissingDataRule()
        {
        }

        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var missingDataResult = MessageLevel.Success;
                var missingDataShipmentNumbers = new List<string>();

                foreach (PrenotificationMovement dto in shipments)
                {
                    if (dto.ActualDateOfShipment.Equals(string.Empty) ||
                    dto.NotificationNumber.Equals(string.Empty) ||
                    dto.PackagingType.Equals(string.Empty) ||
                    dto.Quantity.Equals(string.Empty) ||
                    dto.Unit.Equals(string.Empty))
                    {
                        missingDataResult = MessageLevel.Error;
                        missingDataShipmentNumbers.Add(dto.ShipmentNumber);
                    }
                }

                string errorMessage = GetErrorMessage(missingDataShipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.MissingData, missingDataResult, missingDataShipmentNumbers, errorMessage);
            });
        }

        private string GetErrorMessage(List<string> erroneousShipmentNumbers)
        {
            if (erroneousShipmentNumbers != null && erroneousShipmentNumbers.Count > 0)
            {
                string shipmentNosString = string.Empty;
                foreach (string shipmentNo in erroneousShipmentNumbers)
                {
                    shipmentNosString += string.Concat(shipmentNo, ", ");
                }
                // Remove the final instance of ", "
                shipmentNosString = shipmentNosString.Remove(shipmentNosString.Length - 2);

                return string.Format("Shipment number/s {0}: there is missing data", shipmentNosString);
            }
            return string.Empty;
        }
    }
}
