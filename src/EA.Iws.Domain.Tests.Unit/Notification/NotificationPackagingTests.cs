namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Domain.Notification;
    using Xunit;

    public class NotificationPackagingTests
    {
        [Fact]
        public void CantCreateOtherWithoutDescription()
        {
            Action createPackagingInfo =
                () => PackagingInfo.CreatePackagingInfo(PackagingType.Other);

            Assert.Throws<InvalidOperationException>(createPackagingInfo);
        }

        [Fact]
        public void CantSetOtherDescriptionToNull()
        {
            Action createPackagingInfo =
                () => PackagingInfo.CreateOtherPackagingInfo(null);

            Assert.Throws<ArgumentNullException>(createPackagingInfo);
        }

        [Fact]
        public void CantSetOtherDescriptionToEmptyString()
        {
            Action createPackagingInfo =
                () => PackagingInfo.CreateOtherPackagingInfo(string.Empty);

            Assert.Throws<ArgumentException>(createPackagingInfo);
        }

        [Fact]
        public void CanAddPackagingInfo()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.UpdatePackagingInfo(new[] { PackagingInfo.CreateOtherPackagingInfo("package description") });

            Assert.Equal(1, notification.ShipmentInfo.PackagingInfos.Count());
        }

        [Fact]
        public void UpdatePackagingInfoReplacesItems()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var packagingInfos = new List<PackagingInfo>
            {
                PackagingInfo.CreateOtherPackagingInfo("package description"),
                PackagingInfo.CreatePackagingInfo(PackagingType.Bag)
            };

            var newPackagingInfos = new List<PackagingInfo>
            {
                PackagingInfo.CreatePackagingInfo(PackagingType.Box),
                PackagingInfo.CreatePackagingInfo(PackagingType.Bulk)
            };

            notification.UpdatePackagingInfo(packagingInfos);

            notification.UpdatePackagingInfo(newPackagingInfos);

            Assert.Collection(notification.ShipmentInfo.PackagingInfos,
                item => Assert.Equal(notification.ShipmentInfo.PackagingInfos.ElementAt(0).PackagingType, PackagingType.Box),
                item => Assert.Equal(notification.ShipmentInfo.PackagingInfos.ElementAt(1).PackagingType, PackagingType.Bulk));
        }
    }
}