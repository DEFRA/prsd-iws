namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using Xunit;

    public class TransitStateTests
    {
        private readonly Guid anyGuid = new Guid("DE6CA75B-41B3-4781-ABE9-6BF09C9FC639");

        [Fact]
        public void CanCreateTransitState()
        {
            var transitState = new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 1);

            Assert.IsType<TransitState>(transitState);
        }

        [Fact]
        public void OrdinalPositionCantBeZero()
        {
            Action createTransitState = () => new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 0);

            Assert.Throws<ArgumentOutOfRangeException>("ordinalPosition", createTransitState);
        }

        [Fact]
        public void OrdinalPositionCantBeNegative()
        {
            Action createTransitState = () => new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, -1);

            Assert.Throws<ArgumentOutOfRangeException>("ordinalPosition", createTransitState);
        }
    }
}