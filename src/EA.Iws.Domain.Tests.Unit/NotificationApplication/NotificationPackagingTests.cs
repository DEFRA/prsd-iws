namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Domain.NotificationApplication;
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

            notification.SetPackagingInfo(new[] { PackagingInfo.CreateOtherPackagingInfo("package description") });

            Assert.Equal(1, notification.PackagingInfos.Count());
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

            notification.SetPackagingInfo(packagingInfos);

            notification.SetPackagingInfo(newPackagingInfos);

            Assert.Collection(notification.PackagingInfos,
                item => Assert.Equal(notification.PackagingInfos.ElementAt(0).PackagingType, PackagingType.Box),
                item => Assert.Equal(notification.PackagingInfos.ElementAt(1).PackagingType, PackagingType.Bulk));
        }
    }
}