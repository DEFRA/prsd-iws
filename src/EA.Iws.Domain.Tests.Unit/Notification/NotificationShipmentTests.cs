namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Xunit;

    public class NotificationShipmentTests
    {
        private static NotificationApplication CreateNotificationApplicationWithShipmentInfo()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.SetSpecialHandling(false, null);

            return notification;
        }

        [Fact]
        public void LastDateCantBeBeforeFirstDate()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            var firstDate = new DateTime(2015, 01, 02);
            var lastDate = new DateTime(2015, 01, 01);

            Action addShipmentInfo =
                () =>
                    notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 10, 0.0001M,
                        ShipmentQuantityUnits.Tonnes);

            Assert.Throws<InvalidOperationException>(addShipmentInfo);
        }

        [Fact]
        public void CanAddsShipmentInfo()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void NumberOfShipmentsCantBeZero()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 0, 100, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void QuantityCantBeZero()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 1, 0, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void NumberOfShipmentsCantBeNegative()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, -5, 100, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void QuantityCantBeNegative()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            Action addShipmentDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 1, -5, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentOutOfRangeException>(addShipmentDates);
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCantBeOutside12Months()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(false);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2016, 01, 02);

            Action addShipmentDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 0, 0, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<InvalidOperationException>(addShipmentDates);
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCanBeInside12Months()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(false);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 31);

            notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 1, 1, ShipmentQuantityUnits.Kilogram);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void PreconsentedNotificationDatesCantBeOutside36Months()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2018, 01, 02);

            Action addShipmentDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 1, 1, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<InvalidOperationException>(addShipmentDates);
        }

        [Fact]
        public void PreconsentedNotificationDatesCanBeInside36Months()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            notification.SetPreconsentedRecoveryFacility(true);

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2017, 12, 31);

            notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 1, 1, ShipmentQuantityUnits.Kilogram);

            Assert.True(notification.HasShipmentInfo);
        }

        [Fact]
        public void QuantityRoundedUpTo4DecimalPlaces()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 10, 1.23445M,
                ShipmentQuantityUnits.Tonnes);

            Assert.Equal(1.2345M, notification.ShipmentInfo.Quantity);
        }

        [Fact]
        public void QuantityRoundedDownTo4DecimalPlaces()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = new DateTime(2015, 12, 01);

            notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 10, 1.23444M,
                ShipmentQuantityUnits.Tonnes);

            Assert.Equal(1.2344M, notification.ShipmentInfo.Quantity);
        }

        [Fact]
        public void FirstDateCantBeDateTimeMinValue()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            var firstDate = DateTime.MinValue;
            var lastDate = DateTime.MinValue.AddDays(1);

            Action updateDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 10, 10M, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentException>("firstDate", updateDates);
        }

        [Fact]
        public void LastDateCantBeDateTimeMinValue()
        {
            var notification = CreateNotificationApplicationWithShipmentInfo();

            var firstDate = new DateTime(2015, 01, 01);
            var lastDate = DateTime.MinValue;

            Action updateDates = () => notification.AddShipmentDatesAndQuantityInfo(firstDate, lastDate, 10, 10M, ShipmentQuantityUnits.Kilogram);

            Assert.Throws<ArgumentException>("lastDate", updateDates);
        }
    }
}