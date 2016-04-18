namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Core.Shared;
    using Xunit;

    public class ShipmentQuantityTests
    {
        [Theory]
        [InlineData("100", ShipmentQuantityUnits.Kilograms)]
        [InlineData("100", ShipmentQuantityUnits.Tonnes)]
        [InlineData("100", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("100", ShipmentQuantityUnits.Litres)]
        public void Equals_SameQuantityAndUnit_ReturnsTrue(string quantity, ShipmentQuantityUnits unit)
        {
            var decimalQuantity = Convert.ToDecimal(quantity);

            var unit1 = new ShipmentQuantity(decimalQuantity, unit);
            var unit2 = new ShipmentQuantity(decimalQuantity, unit);

            Assert.True(unit1.Equals(unit2));
        }

        [Theory]
        [InlineData("100", ShipmentQuantityUnits.Kilograms, "0.1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("0.1", ShipmentQuantityUnits.Tonnes, "100", ShipmentQuantityUnits.Kilograms)]
        [InlineData("100", ShipmentQuantityUnits.Litres, "0.1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("0.1", ShipmentQuantityUnits.CubicMetres, "100", ShipmentQuantityUnits.Litres)]
        public void Equals_EquivalentQuantity_ReturnsTrue(string quantity1, ShipmentQuantityUnits unit1,
            string quantity2, ShipmentQuantityUnits unit2)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);

            var shipmentUnit1 = new ShipmentQuantity(decimalQuantity1, unit1);
            var shipmentUnit2 = new ShipmentQuantity(decimalQuantity2, unit2);

            Assert.True(shipmentUnit1.Equals(shipmentUnit2));
        }

        [Theory]
        [InlineData("100", ShipmentQuantityUnits.Kilograms, "0.1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("100", ShipmentQuantityUnits.Kilograms, "0.1", ShipmentQuantityUnits.Litres)]
        [InlineData("100", ShipmentQuantityUnits.Tonnes, "0.1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("100", ShipmentQuantityUnits.Tonnes, "0.1", ShipmentQuantityUnits.Litres)]
        [InlineData("100", ShipmentQuantityUnits.CubicMetres, "0.1", ShipmentQuantityUnits.Kilograms)]
        [InlineData("100", ShipmentQuantityUnits.CubicMetres, "0.1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("100", ShipmentQuantityUnits.Litres, "0.1", ShipmentQuantityUnits.Kilograms)]
        [InlineData("100", ShipmentQuantityUnits.Litres, "0.1", ShipmentQuantityUnits.Tonnes)]
        public void Equals_UnconvertableUnits_ReturnsFalse(string quantity1, ShipmentQuantityUnits unit1,
            string quantity2, ShipmentQuantityUnits unit2)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);

            var shipmentUnit1 = new ShipmentQuantity(decimalQuantity1, unit1);
            var shipmentUnit2 = new ShipmentQuantity(decimalQuantity2, unit2);

            Assert.False(shipmentUnit1.Equals(shipmentUnit2));
        }

        [Theory]
        [InlineData(ShipmentQuantityUnits.Tonnes)]
        [InlineData(ShipmentQuantityUnits.Kilograms)]
        [InlineData(ShipmentQuantityUnits.Litres)]
        [InlineData(ShipmentQuantityUnits.CubicMetres)]
        public void Add_SameUnitType_ReturnsExpected(ShipmentQuantityUnits unit)
        {
            var unit1 = new ShipmentQuantity(1, unit);
            var unit2 = new ShipmentQuantity(2, unit);

            var expected = new ShipmentQuantity(3, unit);

            Assert.Equal(expected, unit1 + unit2);
        }

        [Theory]
        [InlineData("1", ShipmentQuantityUnits.Kilograms, "1", ShipmentQuantityUnits.Tonnes, "1001")]
        [InlineData("1", ShipmentQuantityUnits.Tonnes, "1", ShipmentQuantityUnits.Kilograms, "1.001")]
        [InlineData("1", ShipmentQuantityUnits.Litres, "1", ShipmentQuantityUnits.CubicMetres, "1001")]
        [InlineData("1", ShipmentQuantityUnits.CubicMetres, "1", ShipmentQuantityUnits.Litres, "1.001")]
        public void Add_ConvertableUnitType_ReturnsExpected(string quantity1, ShipmentQuantityUnits units1, string quantity2, ShipmentQuantityUnits units2, string expected)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);
            var decimalExpected = Convert.ToDecimal(expected);

            var unit1 = new ShipmentQuantity(decimalQuantity1, units1);
            var unit2 = new ShipmentQuantity(decimalQuantity2, units2);

            var expectedUnit = new ShipmentQuantity(decimalExpected, units1);

            Assert.Equal(expectedUnit, unit1 + unit2);
        }

        [Theory]
        [InlineData("1001", ShipmentQuantityUnits.Kilograms, "1", ShipmentQuantityUnits.Tonnes, "1")]
        [InlineData("1", ShipmentQuantityUnits.Tonnes, "1", ShipmentQuantityUnits.Kilograms, "0.999")]
        [InlineData("1001", ShipmentQuantityUnits.Litres, "1", ShipmentQuantityUnits.CubicMetres, "1")]
        [InlineData("1", ShipmentQuantityUnits.CubicMetres, "1", ShipmentQuantityUnits.Litres, "0.999")]
        public void Subtract_ConvertableUnitType_ReturnsExpected(string quantity1, ShipmentQuantityUnits units1, string quantity2, ShipmentQuantityUnits units2, string expected)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);
            var decimalExpected = Convert.ToDecimal(expected);

            var unit1 = new ShipmentQuantity(decimalQuantity1, units1);
            var unit2 = new ShipmentQuantity(decimalQuantity2, units2);

            var expectedUnit = new ShipmentQuantity(decimalExpected, units1);

            Assert.Equal(expectedUnit, unit1 - unit2);
        }

        [Theory]
        [InlineData("1001", ShipmentQuantityUnits.Kilograms, "1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("1", ShipmentQuantityUnits.Tonnes, "999", ShipmentQuantityUnits.Kilograms)]
        [InlineData("1001", ShipmentQuantityUnits.Litres, "1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("1", ShipmentQuantityUnits.CubicMetres, "999", ShipmentQuantityUnits.Litres)]
        public void GreaterThan_ConvertableUnitType_ReturnsTrue(string quantity1, ShipmentQuantityUnits units1,
            string quantity2, ShipmentQuantityUnits units2)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);

            var unit1 = new ShipmentQuantity(decimalQuantity1, units1);
            var unit2 = new ShipmentQuantity(decimalQuantity2, units2);

            Assert.True(unit1 > unit2);
        }

        [Theory]
        [InlineData("1", ShipmentQuantityUnits.Tonnes, "1001", ShipmentQuantityUnits.Kilograms)]
        [InlineData("999", ShipmentQuantityUnits.Kilograms, "1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("1", ShipmentQuantityUnits.CubicMetres, "1001", ShipmentQuantityUnits.Litres)]
        [InlineData("999", ShipmentQuantityUnits.Litres, "1", ShipmentQuantityUnits.CubicMetres)]
        public void LessThan_ConvertableUnitType_ReturnsTrue(string quantity1, ShipmentQuantityUnits units1,
            string quantity2, ShipmentQuantityUnits units2)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);

            var unit1 = new ShipmentQuantity(decimalQuantity1, units1);
            var unit2 = new ShipmentQuantity(decimalQuantity2, units2);

            Assert.True(unit1 < unit2);
        }

        [Theory]
        [InlineData("1001", ShipmentQuantityUnits.Kilograms, "1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("1", ShipmentQuantityUnits.Tonnes, "999", ShipmentQuantityUnits.Kilograms)]
        [InlineData("1001", ShipmentQuantityUnits.Litres, "1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("1", ShipmentQuantityUnits.CubicMetres, "999", ShipmentQuantityUnits.Litres)]
        [InlineData("100", ShipmentQuantityUnits.Kilograms, "0.1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("0.1", ShipmentQuantityUnits.Tonnes, "100", ShipmentQuantityUnits.Kilograms)]
        [InlineData("100", ShipmentQuantityUnits.Litres, "0.1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("0.1", ShipmentQuantityUnits.CubicMetres, "100", ShipmentQuantityUnits.Litres)]
        public void GreaterThanOrEquals_ConvertableUnitType_ReturnsTrue(string quantity1, ShipmentQuantityUnits units1,
            string quantity2, ShipmentQuantityUnits units2)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);

            var unit1 = new ShipmentQuantity(decimalQuantity1, units1);
            var unit2 = new ShipmentQuantity(decimalQuantity2, units2);

            Assert.True(unit1 >= unit2);
        }

        [Theory]
        [InlineData("1", ShipmentQuantityUnits.Tonnes, "1001", ShipmentQuantityUnits.Kilograms)]
        [InlineData("999", ShipmentQuantityUnits.Kilograms, "1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("1", ShipmentQuantityUnits.CubicMetres, "1001", ShipmentQuantityUnits.Litres)]
        [InlineData("999", ShipmentQuantityUnits.Litres, "1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("100", ShipmentQuantityUnits.Kilograms, "0.1", ShipmentQuantityUnits.Tonnes)]
        [InlineData("0.1", ShipmentQuantityUnits.Tonnes, "100", ShipmentQuantityUnits.Kilograms)]
        [InlineData("100", ShipmentQuantityUnits.Litres, "0.1", ShipmentQuantityUnits.CubicMetres)]
        [InlineData("0.1", ShipmentQuantityUnits.CubicMetres, "100", ShipmentQuantityUnits.Litres)]
        public void LessThanOrEquals_ConvertableUnitType_ReturnsTrue(string quantity1, ShipmentQuantityUnits units1,
            string quantity2, ShipmentQuantityUnits units2)
        {
            var decimalQuantity1 = Convert.ToDecimal(quantity1);
            var decimalQuantity2 = Convert.ToDecimal(quantity2);

            var unit1 = new ShipmentQuantity(decimalQuantity1, units1);
            var unit2 = new ShipmentQuantity(decimalQuantity2, units2);

            Assert.True(unit1 <= unit2);
        }
    }
}