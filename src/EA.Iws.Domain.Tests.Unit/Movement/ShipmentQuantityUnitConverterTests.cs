namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using Xunit;

    public class ShipmentQuantityUnitConverterTests
    {
        public static IEnumerable<object[]> WorkingConversionTargets
        {
            get
            {
                return new[]
                {
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms, 1, 1000 },
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms, 2, 2000 },
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms, 0, 0 },
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms, 1.5, 1500 },
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms, 0.0005, 0.5 },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Litres, 1, 1000 },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Litres, 2, 2000 },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Litres, 0, 0 },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Litres, 1.5, 1500 },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Litres, 0.0005, 0.5 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 1000, 1 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 10000, 10 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 100000, 100 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 500, 0.5 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 50, 0.05 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 5, 0.005 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 0.5, 0.0005 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 0, 0 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 1000, 1 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 10000, 10 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 100000, 100 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 500, 0.5 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 50, 0.05 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 5, 0.005 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 0.5, 0.0005 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 0, 0 }
                };
            }
        }

        public static IEnumerable<object[]> InvalidConversions
        {
            get
            {
                return new[]
                {
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.CubicMetres },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Litres },
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.CubicMetres },
                    new object[] { ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Litres },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.Kilograms },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.Tonnes },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Kilograms },
                    new object[] { ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Tonnes }
                };
            }
        }

        public static IEnumerable<object[]> LosingPrecisionThrows
        {
            get
            {
                return new[]
                {
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 0.01 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 0.001 },
                    new object[] { ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes, 0.0001 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 0.01 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 0.001 },
                    new object[] { ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres, 0.0001 }
                };
            }
        }

        public static IEnumerable<object[]> CanConvertToSameType
        {
            get
            {
                return new[]
                {
                    new object[] { ShipmentQuantityUnits.Kilograms },
                    new object[] { ShipmentQuantityUnits.Tonnes },
                    new object[] { ShipmentQuantityUnits.CubicMetres },
                    new object[] { ShipmentQuantityUnits.Litres }
                };
            }
        }
        
        [Theory, MemberData("WorkingConversionTargets")]
        public void CheckWorkingConversions(ShipmentQuantityUnits source, ShipmentQuantityUnits target, decimal value, decimal result)
        {
            Assert.Equal(result, ShipmentQuantityUnitConverter.ConvertToTarget(source, target, value));
        }

        [Theory, MemberData("InvalidConversions")]
        public void CheckInvalidConversions(ShipmentQuantityUnits source, ShipmentQuantityUnits target)
        {
            try
            {
                ShipmentQuantityUnitConverter.ConvertToTarget(source, target, 10);
            }
            catch (ArgumentException ex)
            {
                Assert.Contains("Cannot convert type", ex.Message);
            }
        }

        [Theory, MemberData("LosingPrecisionThrows")]
        public void CheckLosingPrecisionThrows(ShipmentQuantityUnits source, ShipmentQuantityUnits target, decimal value)
        {
            try
            {
                ShipmentQuantityUnitConverter.ConvertToTarget(source, target, value);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Contains("would lose precision", ex.Message);
            }
        }

        [Theory, MemberData("LosingPrecisionThrows")]
        public void CanTurnThrowOnLossOfPrecisionOff(ShipmentQuantityUnits source, ShipmentQuantityUnits target,
            decimal value)
        {
            Assert.Equal(value / 1000, ShipmentQuantityUnitConverter.ConvertToTarget(source, target, value, false));
        }

        [Theory, MemberData("CanConvertToSameType")]
        public void ConvertToSameType(ShipmentQuantityUnits sourceAndTarget)
        {
            Assert.Equal(10, ShipmentQuantityUnitConverter.ConvertToTarget(sourceAndTarget, sourceAndTarget, 10));
        }
    }
}
