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

                return new ContentRuleResult<BulkMovementContentRules>(BulkMovementContentRules.HistoricDate, historicDateResult, historicDateShipmentNumbers);
            });
        }
    }
}
