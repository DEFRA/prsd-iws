namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using Prsd.Core.Mapper;
    using Requests.Notification;

    internal class ShipmentQuantityUnitsMap : IMap<ShipmentQuantityUnits, Domain.ShipmentQuantityUnits>
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
    }
}