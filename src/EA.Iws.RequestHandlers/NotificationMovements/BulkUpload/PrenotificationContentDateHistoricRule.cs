namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;

    public class PrenotificationContentDateHistoricRule : IBulkMovementPrenotificationContentRule
    {
        public async Task<ContentRuleResult<BulkMovementContentRules>> GetResult(List<PrenotificationMovement> shipments, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var historicDateResult = MessageLevel.Success;
                var historicDateShipmentNumbers = new List<string>();

                foreach (PrenotificationMovement shipment in shipments)
                {
                    // Only report an error if shipment has a shipment number, otherwise record will be picked up by the PrenotificationContentMissingShipmentNumberRule
                    if (shipment.HasShipmentNumber)
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(shipment.ActualDateOfShipment, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        {
                            if (DateTime.Compare(date, DateTime.UtcNow) < 0)
                            {
                                historicDateResult = MessageLevel.Error;
                                historicDateShipmentNumbers.Add(shipment.ShipmentNumber);
                            }
                        }
                    }
                }

                var shipmentNumbers = string.Join(", ", historicDateShipmentNumbers);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(BulkMovementContentRules.HistoricDate), shipmentNumbers);

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.HistoricDate, historicDateResult, errorMessage);
            });
        }
    }
}
