namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Xunit;

    public class NotificationShipmentTests
    {
        private static NotificationApplication CreateNotificationApplication()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            return notification;
        }

        [Fact]
        public void LastDateCantBeBeforeFirstDate()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 02);
            var lastDate = new DateTime(2015, 01, 01);

            Action updateShipmentInfo =
                () =>
                    notification.UpdateShipmentInfo(firstDate, lastDate, 10, 0.0001M,
                        ShipmentQuantityUnits.Tonnes);

            Assert.Throws<InvalidOperationException>(updateShipmentInfo);
        }

        [Fact]
        public void CanUpdateShipmentInfo()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void NumberOfShipmentsCantBeZero()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 0, 100, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void QuantityCantBeZero()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 1, 0, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void NumberOfShipmentsCantBeNegative()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, -5, 100, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void QuantityCantBeNegative()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 1, -5, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCantBeOutside12Months()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(false);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2016, 01, 02);

            Action addShipmentDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 0, 0, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<InvalidOperationException>(addShipmentDates);
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCanBeInside12Months()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(false);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 31);

            notification.UpdateShipmentInfo(firstDate, lastDate, 1, 1, ShipmentQuantityUnits.Kilogram);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void PreconsentedNotificationDatesCantBeOutside36Months()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2018, 01, 02);

            Action addShipmentDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 1, 1, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<InvalidOperationException>(addShipmentDates);
        }

        [Fact]
        public void PreconsentedNotificationDatesCanBeInside36Months()
        {
            var notification = CreateNotificationApplication();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2017, 12, 31);

            notification.UpdateShipmentInfo(firstDate, lastDate, 1, 1, ShipmentQuantityUnits.Kilogram);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void QuantityRoundedUpTo4DecimalPlaces()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 1.23445M,
                ShipmentQuantityUnits.Tonnes);

            Assert.Equal(1.2345M, notification.ShipmentInfo.Quantity);
        }

        [Fact]
        public void QuantityRoundedDownTo4DecimalPlaces()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 1.23444M,
                ShipmentQuantityUnits.Tonnes);

            Assert.Equal(1.2344M, notification.ShipmentInfo.Quantity);
        }

        [Fact]
        public void FirstDateCantBeDateTimeMinValue()
        {
            var notification = CreateNotificationApplication();

            var firstDate = DateTime.MinValue;
            var lastDate = DateTime.MinValue.AddDays(1);

            Action updateDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 10, 10M, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentException>("firstDate", updateDates);
        }

        [Fact]
        public void LastDateCantBeDateTimeMinValue()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = DateTime.MinValue;

            Action updateDates = () => notification.UpdateShipmentInfo(firstDate, lastDate, 10, 10M, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentException>("lastDate", updateDates);
        }

        [Fact]
        public void CanUpdateShipmentPeriod()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            var newFirstDate = new DateTime(2015, 06, 01);
            var newLastDate = new DateTime(2016, 05, 31);

            notification.UpdateShipmentInfo(newFirstDate, newLastDate, 10, 0.0001M, ShipmentQuantityUnits.Tonnes);

            Assert.Equal(newFirstDate, notification.ShipmentInfo.FirstDate);
            Assert.Equal(newLastDate, notification.ShipmentInfo.LastDate);
        }

        [Fact]
        public void CanUpdateShipmentQuantity()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 2.0M, ShipmentQuantityUnits.Kilogram);

            Assert.Equal(2.0M, notification.ShipmentInfo.Quantity);
            Assert.Equal(ShipmentQuantityUnits.Kilogram, notification.ShipmentInfo.Units);
        }

        [Fact]
        public void CanUpdateNumberOfShipments()
        {
            var notification = CreateNotificationApplication();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.UpdateShipmentInfo(firstDate, lastDate, 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            notification.UpdateShipmentInfo(firstDate, lastDate, 50, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            Assert.Equal(50, notification.ShipmentInfo.NumberOfShipments);
        }
    }
}