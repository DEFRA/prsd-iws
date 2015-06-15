namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Prsd.Core.Mapper;
    using Requests.Shipment;

    internal class ShipmentQuantityUnitsMap : IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits>,
        IMap<Domain.ShipmentQuantityUnits, ShipmentQuantityUnits>
    {
        public Domain.ShipmentQuantityUnits Map(ShipmentQuantityUnits source)
        {
            Domain.ShipmentQuantityUnits unit;

            switch (source)
            {
                case ShipmentQuantityUnits.Tonnes:
                    unit = Domain.ShipmentQuantityUnits.Tonnes;
                    break;
                case ShipmentQuantityUnits.CubicMetres:
                    unit = Domain.ShipmentQuantityUnits.CubicMetres;
                    break;
                case ShipmentQuantityUnits.Litres:
                    unit = Domain.ShipmentQuantityUnits.Litres;
                    break;
                case ShipmentQuantityUnits.Kilograms:
                    unit = Domain.ShipmentQuantityUnits.Kilogram;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Unknown unit: {0}", source));
            }

            return unit;
        }

        public ShipmentQuantityUnits Map(Domain.ShipmentQuantityUnits source)
        {
            ShipmentQuantityUnits unit;
            if (Enum.TryParse(source.Value.ToString(), out unit))
            {
                return unit;
            }
            throw new ArgumentException(string.Format("Unknown ShipmentQuantityUnits {0}", source.Value), "source");
        }
    }
}