namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    public class NotificationShipmentTests : IDisposable
    {
        private static readonly Guid AnyGuid = new Guid("7F4201C4-2BF9-4F09-B3B3-077313A3A871");

        public NotificationShipmentTests()
        {
            // Set "today" at 2014/01/01
            SystemTime.Freeze(new DateTime(2014, 01, 01));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void LastDateCantBeBeforeFirstDate()
        {
            var firstDate = new DateTime(2015, 01, 02);
            var lastDate = new DateTime(2015, 01, 01);

            var shipmentQuantity = new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes);

            Action createShipmentInfo = () =>
                new ShipmentInfo(AnyGuid, new ShipmentPeriod(firstDate, lastDate, true), 1, shipmentQuantity);

            Assert.Throws<InvalidOperationException>(createShipmentInfo);
        }

        [Fact]
        public void CanCreateShipmentInfo()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);
            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes));

            Assert.NotNull(shipmentInfo);
        }

        [Fact]
        public void NumberOfShipmentsCantBeZero()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);
            var shipmentQuantity = new ShipmentQuantity(100, ShipmentQuantityUnits.Kilograms);

            Action createShipmentInfo = () => new ShipmentInfo(AnyGuid, shipmentPeriod, 0, shipmentQuantity);

            Assert.Throws<ArgumentOutOfRangeException>(createShipmentInfo);
        }

        [Fact]
        public void QuantityCantBeZero()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            Action createShipmentInfo = () => new ShipmentInfo(AnyGuid, shipmentPeriod, 1,
                new ShipmentQuantity(0, ShipmentQuantityUnits.Kilograms));

            Assert.Throws<ArgumentOutOfRangeException>(createShipmentInfo);
        }

        [Fact]
        public void NumberOfShipmentsCantBeNegative()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);
            var shipmentQuantity = new ShipmentQuantity(100, ShipmentQuantityUnits.Kilograms);

            Action createShipmentInfo = () => new ShipmentInfo(AnyGuid, shipmentPeriod, -5, shipmentQuantity);

            Assert.Throws<ArgumentOutOfRangeException>(createShipmentInfo);
        }

        [Fact]
        public void QuantityCantBeNegative()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            Action createShipmentInfo = () => new ShipmentInfo(AnyGuid, shipmentPeriod, 1,
                new ShipmentQuantity(-5, ShipmentQuantityUnits.Kilograms));

            Assert.Throws<ArgumentOutOfRangeException>(createShipmentInfo);
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCantBeOutside12Months()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2016, 01, 02);

            var shipmentQuantity = new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes);

            Action createShipmentInfo = () =>
                new ShipmentInfo(AnyGuid, new ShipmentPeriod(firstDate, lastDate, false), 1, shipmentQuantity);

            Assert.Throws<InvalidOperationException>(createShipmentInfo);
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCanBeInside12Months()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 31);
            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(1, ShipmentQuantityUnits.Kilograms));

            Assert.NotNull(shipmentInfo);
        }

        [Fact]
        public void PreconsentedNotificationDatesCantBeOutside36Months()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2018, 01, 02);

            var shipmentQuantity = new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes);

            Action createShipmentInfo = () =>
                new ShipmentInfo(AnyGuid, new ShipmentPeriod(firstDate, lastDate, true), 1, shipmentQuantity);

            Assert.Throws<InvalidOperationException>(createShipmentInfo);
        }

        [Fact]
        public void PreconsentedNotificationDatesCanBeInside36Months()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2017, 12, 31);
            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(1, ShipmentQuantityUnits.Kilograms));

            Assert.NotNull(shipmentInfo);
        }

        [Fact]
        public void QuantityTonnesMoreThan4DecimalPlacesRoundsUp()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(1.23446m, ShipmentQuantityUnits.Tonnes));

            Assert.Equal(1.2345m, shipmentInfo.Quantity);
        }

        [Fact]
        public void QuantityTonnesMoreThan4DecimalPlacesRoundsDown()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(1.23012m, ShipmentQuantityUnits.Tonnes));

            Assert.Equal(1.2301m, shipmentInfo.Quantity);
        }

        [Fact]
        public void QuantityKilogramsMoreThan2DecimalPlaceRoundsUp()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(1.26m, ShipmentQuantityUnits.Kilograms));

            Assert.Equal(1.3m, shipmentInfo.Quantity);
        }

        [Fact]
        public void QuantityKilogramsMoreThan1DecimalPlaceRoundsDown()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(1.23m, ShipmentQuantityUnits.Kilograms));

            Assert.Equal(1.2m, shipmentInfo.Quantity);
        }

        [Fact]
        public void FirstDateCantBeDateTimeMinValue()
        {
            var firstDate = DateTime.MinValue;
            var lastDate = DateTime.MinValue.AddDays(1);

            Action createShipmentInfo = () =>
                new ShipmentInfo(AnyGuid, new ShipmentPeriod(firstDate, lastDate, true), 10,
                new ShipmentQuantity(10M, ShipmentQuantityUnits.Kilograms));

            Assert.Throws<ArgumentException>("firstDate", createShipmentInfo);
        }

        [Fact]
        public void LastDateCantBeDateTimeMinValue()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = DateTime.MinValue;

            Action createShipmentInfo = () =>
                new ShipmentInfo(AnyGuid, new ShipmentPeriod(firstDate, lastDate, true), 10,
                new ShipmentQuantity(10M, ShipmentQuantityUnits.Kilograms));

            Assert.Throws<ArgumentException>("lastDate", createShipmentInfo);
        }

        [Fact]
        public void CanUpdateShipmentPeriod()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);
            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes));

            var newFirstDate = new DateTime(2015, 06, 01);
            var newLastDate = new DateTime(2016, 05, 31);
            var newShipmentPeriod = new ShipmentPeriod(newFirstDate, newLastDate, true);

            shipmentInfo.UpdateShipmentPeriod(newShipmentPeriod);

            Assert.Equal(newFirstDate, shipmentInfo.ShipmentPeriod.FirstDate);
            Assert.Equal(newLastDate, shipmentInfo.ShipmentPeriod.LastDate);
        }

        [Fact]
        public void CanUpdateShipmentQuantity()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);
            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes));

            shipmentInfo.UpdateQuantity(new ShipmentQuantity(2.0M, ShipmentQuantityUnits.Kilograms));

            Assert.Equal(2.0M, shipmentInfo.Quantity);
            Assert.Equal(ShipmentQuantityUnits.Kilograms, shipmentInfo.Units);
        }

        [Fact]
        public void CanUpdateNumberOfShipments()
        {
            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);
            var shipmentPeriod = new ShipmentPeriod(firstDate, lastDate, true);

            var shipmentInfo = new ShipmentInfo(AnyGuid, shipmentPeriod, 10,
                new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes));

            shipmentInfo.UpdateNumberOfShipments(50);

            Assert.Equal(50, shipmentInfo.NumberOfShipments);
        }

        [Fact]
        public void FirstDateCannotBeInThePast()
        {
            // Set "today" at 2015/06/01
            SystemTime.Freeze(new DateTime(2015, 06, 01));

            var firstDate = new DateTime(2015, 01, 02);
            var lastDate = new DateTime(2015, 01, 12);

            Action createShipmentInfo =
                () =>
                     new ShipmentInfo(AnyGuid, new ShipmentPeriod(firstDate, lastDate, true), 10,
                        new ShipmentQuantity(0.0001M, ShipmentQuantityUnits.Tonnes));

            Assert.Throws<InvalidOperationException>(createShipmentInfo);

            SystemTime.Unfreeze();
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCantBeExactly12Months()
        {
            bool preconsentedRecoveryFacility = false;

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2016, 01, 01);

            Action createShipmentInfo = () => new ShipmentInfo(AnyGuid,
                new ShipmentPeriod(firstDate, lastDate, preconsentedRecoveryFacility), 1,
                new ShipmentQuantity(7, ShipmentQuantityUnits.Kilograms));

            Assert.Throws<InvalidOperationException>(createShipmentInfo);
        }

        [Fact]
        public void PreconsentedNotificationDatesCantBeExact36Months()
        {
            bool preconsentedRecoveryFacility = true;

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2018, 01, 01);

            Action createShipmentInfo = () => new ShipmentInfo(AnyGuid,
                new ShipmentPeriod(firstDate, lastDate, preconsentedRecoveryFacility), 1,
                new ShipmentQuantity(7, ShipmentQuantityUnits.Kilograms));

            Assert.Throws<InvalidOperationException>(createShipmentInfo);
        }
    }
}