namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Prsd.Core;

    public class PrenotificationDateHistoricRule : IPrenotificationContentRule
    {
        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue && m.ActualDateOfShipment.HasValue &&
                                m.ActualDateOfShipment.Value.Date < SystemTime.UtcNow.Date)
                        .GroupBy(x => x.ShipmentNumber)
                        .OrderBy(x => x.Key)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;
                var minShipment = shipments.FirstOrDefault() ?? 0;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.HistoricDate), shipmentNumbers);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.HistoricDate, result, errorMessage, minShipment);
            });
        }
    }
}
