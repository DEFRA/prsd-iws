namespace EA.Iws.RequestHandlers.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Prsd.Core;

    public class PrenotificationDateFutureRule : IPrenotificationContentRule
    {
        private const int MaxDays = 30;

        public async Task<PrenotificationContentRuleResult<PrenotificationContentRules>> GetResult(List<PrenotificationMovement> movements, Guid notificationId)
        {
            return await Task.Run(() =>
            {
                var shipments =
                    movements.Where(
                            m =>
                                m.ShipmentNumber.HasValue && m.ActualDateOfShipment.HasValue &&
                                m.ActualDateOfShipment.Value.Date >= SystemTime.UtcNow.Date &&
                                (m.ActualDateOfShipment.Value.Date - SystemTime.UtcNow.Date).TotalDays >= MaxDays) //Equal will include the current date as the first day.
                        .GroupBy(x => x.ShipmentNumber)
                        .Select(x => x.Key)
                        .ToList();

                var result = shipments.Any() ? MessageLevel.Error : MessageLevel.Success;

                var shipmentNumbers = string.Join(", ", shipments);
                var errorMessage = string.Format(Prsd.Core.Helpers.EnumHelper.GetDisplayName(PrenotificationContentRules.FutureDate), shipmentNumbers, MaxDays);

                return new PrenotificationContentRuleResult<PrenotificationContentRules>(PrenotificationContentRules.FutureDate, result, errorMessage);
            });
        }
    }
}
