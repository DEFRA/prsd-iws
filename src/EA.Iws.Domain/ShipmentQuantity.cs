namespace EA.Iws.Domain
{
    using Core.Shared;
    using Prsd.Core;

    public struct ShipmentQuantity
    {
        public decimal Quantity { get; private set; }
        public ShipmentQuantityUnits Units { get; private set; }

        public ShipmentQuantity(decimal quantity, ShipmentQuantityUnits units) : this()
        {
            Guard.ArgumentNotZeroOrNegative(() => quantity, quantity);
            
            Quantity = decimal.Round(quantity, 4);
            Units = units;
        }
    }
}
