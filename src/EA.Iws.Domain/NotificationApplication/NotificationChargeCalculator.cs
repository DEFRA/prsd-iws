namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;

    public class NotificationChargeCalculator
    {
        public decimal GetValue(IEnumerable<PricingStructure> pricingStructures, NotificationApplication notification, ShipmentInfo shipmentInfo)
        {
            if (shipmentInfo == null)
            {
                return 0;
            }

            var pricingStructure = pricingStructures.Single(p =>
                p.CompetentAuthority.Value == notification.CompetentAuthority.Value
                && p.Activity.TradeDirection == TradeDirection.Export
                && p.Activity.NotificationType.Value == notification.NotificationType.Value
                && p.Activity.IsInterim == notification.IsInterim
                && (p.ShipmentQuantityRange.RangeFrom <= shipmentInfo.NumberOfShipments
                    && (p.ShipmentQuantityRange.RangeTo == null
                    || p.ShipmentQuantityRange.RangeTo >= shipmentInfo.NumberOfShipments)));

            return pricingStructure.Price;
        }
    }
}
