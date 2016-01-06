namespace EA.Iws.Domain
{
    using System;
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

        public override bool Equals(object obj)
        {
            var shipmentQuantity = obj as ShipmentQuantity;
            if (shipmentQuantity != null)
            {
                return Equals(shipmentQuantity);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Quantity.GetHashCode() * 397) ^ (int)Units;
            }
        }

        public bool Equals(ShipmentQuantity other)
        {
            if (this.Units == other.Units)
            {
                return this.Quantity == other.Quantity;
            }

            try
            {
                var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(other.Units, this.Units, other.Quantity);
                return this.Quantity == quantity;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
               return false;
            }
        }

        public static bool operator ==(ShipmentQuantity x, ShipmentQuantity y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(ShipmentQuantity x, ShipmentQuantity y)
        {
            return !(x == y);
        }

        public static bool operator <(ShipmentQuantity x, ShipmentQuantity y)
        {
            var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(y.Units, x.Units, y.Quantity);
            return x.Quantity < quantity;
        }

        public static bool operator >(ShipmentQuantity x, ShipmentQuantity y)
        {
            var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(y.Units, x.Units, y.Quantity);
            return x.Quantity > quantity;
        }

        public static bool operator <=(ShipmentQuantity x, ShipmentQuantity y)
        {
            var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(y.Units, x.Units, y.Quantity);
            return x.Quantity <= quantity;
        }

        public static bool operator >=(ShipmentQuantity x, ShipmentQuantity y)
        {
            var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(y.Units, x.Units, y.Quantity);
            return x.Quantity >= quantity;
        }

        public static ShipmentQuantity operator +(ShipmentQuantity x, ShipmentQuantity y)
        {
            var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(y.Units, x.Units, y.Quantity);
            return new ShipmentQuantity(x.Quantity + quantity, x.Units);
        }

        public static ShipmentQuantity operator -(ShipmentQuantity x, ShipmentQuantity y)
        {
            var quantity = ShipmentQuantityUnitConverter.ConvertToTarget(y.Units, x.Units, y.Quantity);
            return new ShipmentQuantity(x.Quantity - quantity, x.Units);
        }
    }
}