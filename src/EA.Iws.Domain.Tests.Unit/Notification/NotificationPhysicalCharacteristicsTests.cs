namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NotificationApplication;
    using Xunit;

    public class NotificationPhysicalCharacteristicsTests
    {
        private readonly NotificationApplication notification;

        public NotificationPhysicalCharacteristicsTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void CantCreateOtherWithoutDescription()
        {
            Action createPhysicalCharacteristic =
                () => PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Other);

            Assert.Throws<InvalidOperationException>(createPhysicalCharacteristic);
        }

        [Fact]
        public void CantSetOtherDescriptionToNull()
        {
            Action createPhysicalCharacteristic =
                () => PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo(null);

            Assert.Throws<ArgumentNullException>("otherDescription", createPhysicalCharacteristic);
        }

        [Fact]
        public void CantSetOtherDescriptionToEmptyString()
        {
            Action createPhysicalCharacteristic =
                () => PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo(string.Empty);

            Assert.Throws<ArgumentException>("otherDescription", createPhysicalCharacteristic);
        }

        [Fact]
        public void CanSetPhysicalCharacteristics()
        {
            notification.SetPhysicalCharacteristics(new[] { PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo("package description") });

            Assert.Equal(1, notification.PhysicalCharacteristics.Count());
        }

        [Fact]
        public void UpdatePhysicalCharacteristicsReplacesItems()
        {
            var physicalCharacteristics = new List<PhysicalCharacteristicsInfo>
            {
                PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo("description"),
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Gaseous)
            };

            var newPhysicalCharacteristics = new List<PhysicalCharacteristicsInfo>
            {
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Liquid),
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Powdery)
            };

            notification.SetPhysicalCharacteristics(physicalCharacteristics);

            notification.SetPhysicalCharacteristics(newPhysicalCharacteristics);

            Assert.Collection(notification.PhysicalCharacteristics,
                item => Assert.Equal(notification.PhysicalCharacteristics.ElementAt(0).PhysicalCharacteristic, PhysicalCharacteristicType.Liquid),
                item => Assert.Equal(notification.PhysicalCharacteristics.ElementAt(1).PhysicalCharacteristic, PhysicalCharacteristicType.Powdery));
        }
    }
}