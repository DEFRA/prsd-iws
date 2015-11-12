namespace EA.Iws.Domain
{
    using Core.Shared;
    using Prsd.Core;

    public class ShipmentQuantity
    {
        public decimal Quantity { get; private set; }
        public ShipmentQuantityUnits Units { get; private set; }

        protected ShipmentQuantity()
        {
        }

        public ShipmentQuantity(decimal quantity, ShipmentQuantityUnits units)
        {
            Guard.ArgumentNotZeroOrNegative(() => quantity, quantity);
            
            Quantity = decimal.Round(quantity, ShipmentQuantityUnitsMetadata.Precision[units]);
            Units = units;
        }
    }
}
