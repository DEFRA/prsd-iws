namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentDateFormatRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var invalidFormatResult = MessageLevel.Success;
                var invalidFormatShipmentNumbers = new List<string>();

                foreach (PrenotificationMovement shipment in shipments)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (shipment.HasShipmentNumber)
                    {
                        DateTime date;
                        if (!DateTime.TryParseExact(shipment.ActualDateOfShipment, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            invalidFormatResult = MessageLevel.Error;
                            invalidFormatShipmentNumbers.Add(shipment.ShipmentNumber);
                        }
                    }
                }

                var shipmentNumbers = string.Join(", ", invalidFormatShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.InvalidDateFormat), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidDateFormat, invalidFormatResult, errorMessage);
            });
        }
    }
}
