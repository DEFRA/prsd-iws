namespace EA.Iws.Domain
{
    using Prsd.Core.Domain;

    public class ShipmentQuantityUnits : Enumeration
    {
        public static readonly ShipmentQuantityUnits NotSet = new ShipmentQuantityUnits(0, "Not Set");
        public static readonly ShipmentQuantityUnits Tonnes = new ShipmentQuantityUnits(1, "Tonnes(Mg)");
        public static readonly ShipmentQuantityUnits CubicMetres = new ShipmentQuantityUnits(2, "m3");
        public static readonly ShipmentQuantityUnits Kilogram = new ShipmentQuantityUnits(3, "Kgs");
        public static readonly ShipmentQuantityUnits Litres = new ShipmentQuantityUnits(4, "Ltrs");

        protected ShipmentQuantityUnits()
        {
        }

        private ShipmentQuantityUnits(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
