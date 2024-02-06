namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    public class ShipmentTests
    {
        private static readonly Guid ImportNotificationId = new Guid("4A339E98-CF7A-4CF9-89C0-C673A32FF124");
        private readonly ShipmentQuantity quantity;
        private readonly ShipmentPeriod period;

        public ShipmentTests()
        {
            quantity = new ShipmentQuantity(50, ShipmentQuantityUnits.Tonnes);
            period = new ShipmentPeriod(new DateTime(2016, 1, 1), new DateTime(2017, 1, 1), true);
        }

        [Fact]
        public void CanCreateShipment()
        {
            var shipment = new Shipment(ImportNotificationId, period, quantity, 50);

            Assert.IsType<Shipment>(shipment);
        }

        [Fact]
        public void ImportNotificationIdCantBeEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Shipment(new Guid(), period, quantity, 50));
        }

        [Fact]
        public void ShipmentPeriodCantBeEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new Shipment(ImportNotificationId, null, quantity, 50));
        }

        [Fact]
        public void ShipmentQuantityCantBeEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new Shipment(ImportNotificationId, period, null, 50));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void NumberOfShipmentsCantBeZeroOrNegative(int numberOfShipments)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Shipment(ImportNotificationId, period, quantity, numberOfShipments));
        }

        [Fact]
        public void WhenFacilitiesArePreconsented_ShipmentPeriodInside36MonthsAllowed()
        {
        }

        [Fact]
        public void WhenFacilitiesArePreconsented_ShipmentPeriodOutside36MonthsNotAllowed()
        {
        }

        [Fact]
        public void WhenFacilitiesAreNotPreconsented_ShipmentPeriodInside12MonthsAllowed()
        {
        }

        [Fact]
        public void WhenFacilitiesAreNotPreconsented_ShipmentPeriodOutside12MonthsNotAllowed()
        {
        }
    }
}