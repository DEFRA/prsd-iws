namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Domain;
    using Prsd.Core.Mapper;

    internal class ShipmentMap : IMapWithParameter<Shipment, Preconsented, Domain.ImportNotification.Shipment>
    {
        public Domain.ImportNotification.Shipment Map(Shipment source, Preconsented parameter)
        {
            return new Domain.ImportNotification.Shipment(source.ImportNotificationId,
                new ShipmentPeriod(source.StartDate.Value, source.EndDate.Value, parameter.AllFacilitiesPreconsented.GetValueOrDefault()),
                new ShipmentQuantity(source.Quantity.Value, source.Unit.Value),
                source.TotalShipments.Value);
        }
    }
}