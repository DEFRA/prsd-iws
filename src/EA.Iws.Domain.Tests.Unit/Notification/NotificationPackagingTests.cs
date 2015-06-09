namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using Domain.Notification;
    using Xunit;

    public class NotificationPackagingTests
    {
        [Fact]
        public void CreatePackagingInfo_WithInvalidData_ThrowsException()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddPackagingInfo(PackagingType.Bulk);

            notification.SetSpecialHandling(false, string.Empty);

            notification.AddShipmentDatesAndQuantityInfo(DateTime.Now, DateTime.Now.AddDays(1), 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            Action addPackagingInfo =
                () => notification.ShipmentInfo.AddPackagingInfo(PackagingType.Bag, "Limited Company");

            Assert.Throws<InvalidOperationException>(addPackagingInfo);
        }

        [Fact]
        public void CreatePackagingInfo_WithValidData_PackagingInfoAdded()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            notification.AddPackagingInfo(PackagingType.Other, "Limited Company");

            notification.SetSpecialHandling(false, string.Empty);

            notification.AddShipmentDatesAndQuantityInfo(DateTime.Now, DateTime.Now.AddDays(1), 10, 0.0001M,
                ShipmentQuantityUnits.Tonnes);

            Assert.Equal(1, notification.ShipmentInfo.PackagingInfos.Count());
        }
    }
}