namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementDetailsTests
    {
        private static readonly Guid MovementId = new Guid("F1E2311D-2016-445D-AF72-6E4F640433B4");
        private static readonly ShipmentQuantity shipmentQuantity = new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms);
        private static readonly PackagingInfo[] packagingInfos = { TestablePackagingInfo.Box, TestablePackagingInfo.LargeSacks };

        [Fact]
        public void NullShipmentQuantityThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MovementDetails(MovementId, null, packagingInfos));
        }

        [Fact]
        public void EmptyPackagingInfosThrows()
        {
            var emptyPackagingInfos = new PackagingInfo[0];

            Assert.Throws<ArgumentException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, emptyPackagingInfos));
        }

        [Fact]
        public void NullPackagingInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, null));
        }

        [Fact]
        public void DefaultMovementIdThrows()
        {
            Assert.Throws<ArgumentException>(() =>
                new MovementDetails(new Guid(), shipmentQuantity, packagingInfos));
        }
    }
}