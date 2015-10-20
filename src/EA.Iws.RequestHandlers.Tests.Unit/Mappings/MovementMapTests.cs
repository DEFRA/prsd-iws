namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System;
    using RequestHandlers.Mappings.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementMapTests
    {
        private readonly MovementProgressMap map;
        private readonly TestableMovement movement;

        public MovementMapTests()
        {
            map = new MovementProgressMap();
            movement = new TestableMovement();
        }

        [Fact]
        public void NullMovement_ReturnsAllFalse()
        {
            var result = map.Map(null);

            Assert.False(result.AreIntendedCarriersCompleted);
            Assert.False(result.ArePackagingTypesCompleted);
            Assert.False(result.IsActualDateCompleted);
            Assert.False(result.IsActualQuantityCompleted);
            Assert.False(result.IsNumberOfPackagesCompleted);
        }

        [Fact]
        public void ActualDateNull_ReturnsFalse()
        {
            movement.Date = null;

            var result = map.Map(movement);

            Assert.False(result.IsActualDateCompleted);
        }

        [Fact]
        public void ActualDateHasValue_ReturnsTrue()
        {
            movement.Date = new DateTime(2015, 1, 1);

            var result = map.Map(movement);

            Assert.True(result.IsActualDateCompleted);
        }

        [Fact]
        public void ActualQuantityNull_ReturnsFalse()
        {
            movement.Quantity = null;

            var result = map.Map(movement);

            Assert.False(result.IsActualQuantityCompleted);
        }

        [Fact]
        public void ActualQuantityHasValue_ReturnsTrue()
        {
            movement.Quantity = 10;

            var result = map.Map(movement);

            Assert.True(result.IsActualQuantityCompleted);
        }

        [Fact]
        public void NumberOfPackagesNull_ReturnsFalse()
        {
            movement.NumberOfPackages = null;

            var result = map.Map(movement);

            Assert.False(result.IsNumberOfPackagesCompleted);
        }

        [Fact]
        public void NumberOfPackagesHasValue_ReturnsTrue()
        {
            movement.NumberOfPackages = 10;

            var result = map.Map(movement);

            Assert.True(result.IsNumberOfPackagesCompleted);
        }

        [Fact]
        public void PackagingInfosIsNull_ReturnsFalse()
        {
            movement.PackagingInfos = null;

            var result = map.Map(movement);

            Assert.False(result.ArePackagingTypesCompleted);
        }

        [Fact]
        public void PackagingInfosIsEmpty_ReturnsFalse()
        {
            movement.PackagingInfos = new TestablePackagingInfo[0];

            var result = map.Map(movement);

            Assert.False(result.ArePackagingTypesCompleted);
        }

        [Fact]
        public void PackagingInfosIsPopulated_ReturnsTrue()
        {
            movement.PackagingInfos = new[]
            {
                new TestablePackagingInfo() 
            };

            var result = map.Map(movement);

            Assert.True(result.ArePackagingTypesCompleted);
        }

        [Fact]
        public void CarriersIsNull_ReturnsFalse()
        {
            movement.MovementCarriers = null;

            var result = map.Map(movement);

            Assert.False(result.AreIntendedCarriersCompleted);
        }

        [Fact]
        public void CarriersIsEmpty_ReturnsFalse()
        {
            movement.MovementCarriers = new TestableMovementCarrier[0];

            var result = map.Map(movement);

            Assert.False(result.AreIntendedCarriersCompleted);
        }

        [Fact]
        public void CarriersIsPopulated_ReturnsTrue()
        {
            movement.MovementCarriers = new[]
            {
                new TestableMovementCarrier()
            };

            var result = map.Map(movement);

            Assert.True(result.AreIntendedCarriersCompleted);
        }
    }
}
