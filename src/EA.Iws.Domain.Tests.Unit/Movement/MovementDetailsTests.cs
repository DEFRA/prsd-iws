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
        private static readonly MovementCarrier[] carriers = { TestableMovementCarrier.MikeMerrysMovers };
        private static readonly PackagingInfo[] packagingInfos = { TestablePackagingInfo.Box, TestablePackagingInfo.LargeSacks };

        [Fact]
        public void NullShipmentQuantityThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MovementDetails(MovementId, null, 5, carriers, packagingInfos));
        }

        [Fact]
        public void EmptyCarriersThrows()
        {
            var emptyCarriers = new MovementCarrier[0];

            Assert.Throws<ArgumentException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, 5, emptyCarriers, packagingInfos));
        }

        [Fact]
        public void NullCarriersThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, 5, null, packagingInfos));
        }

        [Fact]
        public void EmptyPackagingInfosThrows()
        {
            var emptyPackagingInfos = new PackagingInfo[0];

            Assert.Throws<ArgumentException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, 5, carriers, emptyPackagingInfos));
        }

        [Fact]
        public void NullPackagingInfoThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, 5, carriers, null));
        }

        [Fact]
        public void ZeroPackagesThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, 0, carriers, packagingInfos));
        }

        [Fact]
        public void NegativePackagesThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new MovementDetails(MovementId, shipmentQuantity, -5, carriers, packagingInfos));
        }

        [Fact]
        public void DefaultMovementIdThrows()
        {
            Assert.Throws<ArgumentException>(() =>
                new MovementDetails(new Guid(), shipmentQuantity, 5, carriers, packagingInfos));
        }
    }
}