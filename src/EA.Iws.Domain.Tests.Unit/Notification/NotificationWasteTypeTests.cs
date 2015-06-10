namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Notification;
    using Xunit;

    public class NotificationWasteTypeTests
    {
        [Fact]
        public void CanAddWasteTypeForWood()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, string.Empty,
                "This waste type is of wood type. I am writing some description here.", null);

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForOther()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Other, "some other name",
                "This waste type is of any other type. I am writing some description here.", null);

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForRdf()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            List<WasteComposition> wasteCompositions = new List<WasteComposition>();
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("First Constituent", 1, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Second Constituent", 2, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Third Constituent", 3, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fourth Constituent", 4, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fifth Constituent", 5, 100));

            notification.AddWasteType(ChemicalComposition.RDF, string.Empty, string.Empty, wasteCompositions);

            Assert.True(notification.HasWasteType);
            Assert.True(5 == notification.WasteType.WasteCompositions.Count());
        }

        [Fact]
        public void CanAddWasteTypeForSrf()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);

            List<WasteComposition> wasteCompositions = new List<WasteComposition>();
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("First Constituent", 1, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Second Constituent", 2, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Third Constituent", 3, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fourth Constituent", 4, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fifth Constituent", 5, 100));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Sixth Constituent", 6, 100));

            notification.AddWasteType(ChemicalComposition.SRF, string.Empty, string.Empty, wasteCompositions);

            Assert.True(notification.HasWasteType);
            Assert.True(6 == notification.WasteType.WasteCompositions.Count());
        }

        [Fact]
        public void CantAddMultipleWasteTypes()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, string.Empty, "some description", null);

            Action addSecondWasteType = () => notification.AddWasteType(ChemicalComposition.RDF, string.Empty, string.Empty, null);

            Assert.Throws<InvalidOperationException>(addSecondWasteType);
        }

        [Fact]
        public void CantAddWasteTypeForWoodWithoutDescription()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addWoodWasteTypeWithoutDescription = () => notification.AddWasteType(ChemicalComposition.Wood, "name", null, null);
            Assert.Throws<ArgumentNullException>(addWoodWasteTypeWithoutDescription);
        }

        [Fact]
        public void CantAddWasteTypeForOtherWithoutOtherName()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutName = () => notification.AddWasteType(ChemicalComposition.Other, null, "description", null);
            Assert.Throws<ArgumentNullException>(addOtherWasteTypeWithoutName);
            var a = notification.WasteType == null;
        }

        [Fact]
        public void CantAddWasteTypeForOtherWithoutDescription()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutDescription = () => notification.AddWasteType(ChemicalComposition.Other, "name", null, null);
            Assert.Throws<ArgumentNullException>(addOtherWasteTypeWithoutDescription);
        }

        [Fact]
        public void CantAddWasteTypeForRdfWithoutComposition()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutDescription = () => notification.AddWasteType(ChemicalComposition.RDF, string.Empty, null, null);
            Assert.Throws<ArgumentException>(addOtherWasteTypeWithoutDescription);
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithValidData_PhysicalCharacteristicsAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description", null);

            notification.AddPhysicalCharacteristic(PhysicalCharacteristicType.Powdery);

            Assert.Equal(1, notification.WasteType.PhysicalCharacteristics.Count());
        }

        [Fact]
        public void AddPhysicalCharacteristics_WithValidDescription_PhysicalCharacteristicsAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description", null);

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

            notification.AddWasteType(ChemicalComposition.Wood, "Name", "Description", null);

            notification.AddWasteGenerationProcess("Process description", true);

            Assert.Equal("Process description", notification.WasteType.WasteGenerationProcess);
        }
    }
}
