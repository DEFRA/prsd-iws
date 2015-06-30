namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationWasteTypeTests
    {
        [Fact]
        public void CanAddWasteTypeForWood()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateRdfWasteType(GetWasteTypeCollection()));

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForOther()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateRdfWasteType(GetWasteTypeCollection()));

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForRdf()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateRdfWasteType(GetWasteTypeCollection()));

            Assert.True(notification.HasWasteType);
            Assert.True(5 == notification.WasteType.WasteCompositions.Count());
        }

        [Fact]
        public void CanAddWasteTypeForSrf()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateSrfWasteType(GetWasteTypeCollection()));

            Assert.True(notification.HasWasteType);
            Assert.True(5 == notification.WasteType.WasteCompositions.Count());
        }

        [Fact]
        public void CantAddMultipleWasteTypes()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateWoodWasteType("some description", GetWasteTypeCollection()));

            Action addSecondWasteType = () => notification.AddWasteType(WasteType.CreateOtherWasteType("name"));

            Assert.Throws<InvalidOperationException>(addSecondWasteType);
        }

        [Fact]
        public void CantAddWasteTypeForOtherWithoutOtherName()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action addOtherWasteTypeWithoutName = () => notification.AddWasteType(WasteType.CreateOtherWasteType(null));
            Assert.Throws<ArgumentNullException>(addOtherWasteTypeWithoutName);
            var a = notification.WasteType == null;
        }

        [Fact]
        public void CanAddWasteTypeForRdfWithoutComposition()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddWasteType(WasteType.CreateRdfWasteType(null));
            Assert.NotNull(notification.WasteType);
        }

        private List<WasteComposition> GetWasteTypeCollection()
        {
            List<WasteComposition> wasteCompositions = new List<WasteComposition>();
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("First Constituent", 1, 100, ChemicalCompositionCategory.Metals));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Second Constituent", 2, 100, ChemicalCompositionCategory.Wood));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Third Constituent", 3, 100, ChemicalCompositionCategory.Food));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fourth Constituent", 4, 100, ChemicalCompositionCategory.Textiles));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fifth Constituent", 5, 100, ChemicalCompositionCategory.Plastics));
            return wasteCompositions;
        }
    }
}
