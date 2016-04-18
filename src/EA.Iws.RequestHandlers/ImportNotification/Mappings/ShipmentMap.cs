namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ShipmentMap : IMap<Domain.Shipment, Core.IntendedShipment>
    {
        public Core.IntendedShipment Map(Domain.Shipment source)
        {
            return new Core.IntendedShipment
            {
                Start = source.Period.FirstDate,
                End = source.Period.LastDate,
                TotalShipments = source.NumberOfShipments,
                Quantity = source.Quantity.Quantity,
                Units = source.Quantity.Units
            };
        }
    }
}