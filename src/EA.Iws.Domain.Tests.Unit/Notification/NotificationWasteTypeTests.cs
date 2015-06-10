namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Xunit;

    public class NotificationWasteTypeTests
    {
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
        public void AddPhysicalCharacteristics_WithDescriptionWhenNotOther_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addPhysicalInfo = () => notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery, "other description");

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
