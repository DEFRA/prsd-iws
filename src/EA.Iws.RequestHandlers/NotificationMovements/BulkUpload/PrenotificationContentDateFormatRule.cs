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
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments)
        {
            return await Task.Run(() =>
            {
                var invalidFormatResult = MessageLevel.Success;
                var invalidFormatShipmentNumbers = new List<string>();

                foreach (PrenotificationMovement shipment in shipments)
                {
                    DateTime date;
                    if (!DateTime.TryParseExact(shipment.ActualDateOfShipment, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        invalidFormatResult = MessageLevel.Error;
                        invalidFormatShipmentNumbers.Add(shipment.ShipmentNumber);
                    }
                }

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.InvalidDateFormat, invalidFormatResult, invalidFormatShipmentNumbers);
            });
        }
    }
}
