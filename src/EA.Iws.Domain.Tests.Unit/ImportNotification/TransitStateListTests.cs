namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using Xunit;

    public class TransitStateListTests
    {
        private readonly Guid anyGuid = new Guid("DE6CA75B-41B3-4781-ABE9-6BF09C9FC639");

        [Fact]
        public void CanCreateTransitStateList()
        {
            var transitStates = new[]
            {
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 1),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 2),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 3)
            };

            var transitStateList = new TransitStateList(transitStates);

            Assert.IsType<TransitStateList>(transitStateList);
        }

        [Fact]
        public void CantHaveDuplicateOrdinalPositions()
        {
            var transitStates = new[]
            {
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 1),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 1),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 3)
            };

            Action createTransitStateList = () => new TransitStateList(transitStates);

            Assert.Throws<ArgumentException>("transitStates", createTransitStateList);
        }

        [Fact]
        public void CantHaveNonsequentialOrdinalPositions()
        {
            var transitStates = new[]
            {
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 1),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 3),
                new TransitState(anyGuid, anyGuid, anyGuid, anyGuid, 4)
            };

            Action createTransitStateList = () => new TransitStateList(transitStates);

            Assert.Throws<ArgumentException>("transitStates", createTransitStateList);
        }
    }
}