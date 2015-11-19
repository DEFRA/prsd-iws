namespace EA.Iws.Domain.NotificationApplication
{
    using System.Collections.Generic;
    using Shipment;

    public interface INotificationChargeCalculator
    {
        decimal GetValue(IEnumerable<PricingStructure> pricingStructures, NotificationApplication notification, ShipmentInfo shipmentInfo);
    }
}