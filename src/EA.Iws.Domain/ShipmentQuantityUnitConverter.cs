namespace EA.Iws.Domain
{
    using System;
    using Core.Shared;

    public static class ShipmentQuantityUnitConverter
    {
        private const decimal ConversionFactor = 1000m;

        private static readonly Action<ShipmentQuantityUnits, ShipmentQuantityUnits> ThrowOnInvalidConversion
            = (source, target) => { throw new ArgumentException("Cannot convert type " + source + " to type " + target); };

        private static readonly Action<ShipmentQuantityUnits, ShipmentQuantityUnits, decimal> ThrowOnPrecisionLoss
            = (source, target, value) => { throw new InvalidOperationException("The conversion of type " + source + " to type " + target + " for " + value + " would lose precision."); };
        
        public static decimal ConvertToTarget(ShipmentQuantityUnits source, ShipmentQuantityUnits target, decimal value, bool throwOnLossOfPrecision = true)
        {
            if (source == target)
            {
                return value;
            }

            if (ShipmentQuantityUnitsMetadata.IsWeightUnit(source) && ShipmentQuantityUnitsMetadata.IsVolumeUnit(target))
            {
                ThrowOnInvalidConversion(source, target);
            }

            if (ShipmentQuantityUnitsMetadata.IsVolumeUnit(source) && ShipmentQuantityUnitsMetadata.IsWeightUnit(target))
            {
                ThrowOnInvalidConversion(source, target);
            }

            if (source == ShipmentQuantityUnits.Tonnes && target == ShipmentQuantityUnits.Kilograms)
            {
                return TonnesToKilograms(value);
            }

            if (source == ShipmentQuantityUnits.CubicMetres && target == ShipmentQuantityUnits.Litres)
            {
                return CubicMetersToLitres(value);
            }

            if (source == ShipmentQuantityUnits.Kilograms && target == ShipmentQuantityUnits.Tonnes)
            {
                var convertedValue = KilogramsToTonnes(value);

                if (WouldLosePrecision(convertedValue, throwOnLossOfPrecision))
                {
                    ThrowOnPrecisionLoss(source, target, value);
                }

                return convertedValue;
            }

            if (source == ShipmentQuantityUnits.Litres && target == ShipmentQuantityUnits.CubicMetres)
            {
                var convertedValue = LitresToCubicMeters(value);

                if (WouldLosePrecision(convertedValue, throwOnLossOfPrecision))
                {
                    ThrowOnPrecisionLoss(source, target, value);
                }

                return convertedValue;
            }

            ThrowOnInvalidConversion(source, target);

            throw new InvalidOperationException();
        }

        private static bool WouldLosePrecision(decimal value, bool throwOnLossOfPrecision)
        {
            return throwOnLossOfPrecision && HasMoreThanNDecimalPlaces(value, 4);
        }

        private static bool HasMoreThanNDecimalPlaces(decimal value, int decimalPlaces)
        {
            return Decimal.Round(value, decimalPlaces) != value;
        }

        private static decimal KilogramsToTonnes(decimal value)
        {
            return value / ConversionFactor;
        }

        private static decimal TonnesToKilograms(decimal value)
        {
            return value * ConversionFactor;
        }

        private static decimal LitresToCubicMeters(decimal value)
        {
            return value / ConversionFactor;
        }

        private static decimal CubicMetersToLitres(decimal value)
        {
            return value * ConversionFactor;
        }
    }
}
