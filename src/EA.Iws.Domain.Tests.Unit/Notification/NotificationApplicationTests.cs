namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationApplicationTests
    {
        private const string England = "England";
        private const string Scotland = "Scotland";
        private const string NorthernIreland = "Northern Ireland";
        private const string Wales = "Wales";

        [Theory]
        [InlineData("GB 0001 123456", England, 123456)]
        [InlineData("GB 0002 123456", Scotland, 123456)]
        [InlineData("GB 0003 123456", NorthernIreland, 123456)]
        [InlineData("GB 0004 123456", Wales, 123456)]
        [InlineData("GB 0001 005000", England, 5000)]
        [InlineData("GB 0002 000100", Scotland, 100)]
        public void NotificationNumberFormat(string expected, string country, int notificationNumber)
        {
            var userId = new Guid("{FCCC2E8A-2464-4C10-8521-09F16F2C550C}");
            var notification = new NotificationApplication(userId, NotificationType.Disposal,
                GetCompetentAuthority(country),
                notificationNumber);
            Assert.Equal(expected, notification.NotificationNumber);
        }

        private static UKCompetentAuthority GetCompetentAuthority(string country)
        {
            if (country == England)
            {
                return UKCompetentAuthority.England;
            }
            if (country == Scotland)
            {
                return UKCompetentAuthority.Scotland;
            }
            if (country == NorthernIreland)
            {
                return UKCompetentAuthority.NorthernIreland;
            }
            if (country == Wales)
            {
                return UKCompetentAuthority.Wales;
            }
            throw new ArgumentException("Unknown competent authority", "country");
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithValidData_PhysicalCharacteristicsAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description");

            notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery);

            Assert.Equal(1, notification.WasteType.PhysicalCharacteristics.Count());
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithInvalidDescription_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description");

            Action addPhysicalInfo = () => notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery, "Other description");

            Assert.Throws<InvalidOperationException>(addPhysicalInfo);
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithValidDescription_PhysicalCharacteristicsAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description");

            notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Other, "Other description");

            Assert.Equal(1, notification.WasteType.PhysicalCharacteristics.Count());
        }

        [Fact] 
        public void AddPhysicalCharacteristics_WithoutWasteType_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addPhysicalInfo = () => notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery);

            Assert.Throws<InvalidOperationException>(addPhysicalInfo);
        }

        [Fact]
        public void AddWasteGenerationProcess_WithoutWasteType_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addWasteGenerationProcess = () => notification.AddWasteGenerationProcess("process", true);

            Assert.Throws<InvalidOperationException>(addWasteGenerationProcess);
        }

        [Fact]
        public void AddWasteGenerationProcess_WithValidData_WasteGenerationDescriptionAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description");

            notification.AddWasteGenerationProcess("Process description", true);

            Assert.Equal("Process description", notification.WasteType.WasteGenerationProcess);
        }
    }
}