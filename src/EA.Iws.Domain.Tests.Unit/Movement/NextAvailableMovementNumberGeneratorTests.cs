namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class NextAvailableMovementNumberGeneratorTests
    {
        private static readonly Guid NotificationId = new Guid("C71388D8-726D-417E-8A05-98FF9B1F451A");
        private readonly IMovementRepository movementRepository;
        private readonly List<Movement> movements; 
        private readonly INextAvailableMovementNumberGenerator nextAvailableMovementNumberGenerator;

        public NextAvailableMovementNumberGeneratorTests()
        {
            movementRepository = A.Fake<IMovementRepository>();
            movements = new List<Movement>();
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(movements);
            nextAvailableMovementNumberGenerator = new NextAvailableMovementNumberGenerator(movementRepository);
        }

        [Fact]
        public async Task NoMovements_ReturnsOne()
        {
            var result = await nextAvailableMovementNumberGenerator.GetNext(NotificationId);

            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(new[] { 1, 3 }, 2)]
        [InlineData(new[] { 1, 2 }, 3)]
        [InlineData(new[] { 1 }, 2)]
        [InlineData(new[] { 2, 3, 4 }, 1)]
        [InlineData(new[] { 1, 2, 4, 6 }, 3)]
        public async Task MovementRange_ReturnsExpectedResult(IEnumerable<int> currentMovements, int expected)
        {
            foreach (var currentMovement in currentMovements)
            {
                movements.Add(GenerateMovementWithNumber(currentMovement));
            }

            var result = await nextAvailableMovementNumberGenerator.GetNext(NotificationId);

            Assert.Equal(expected, result);
        }

        private Movement GenerateMovementWithNumber(int number)
        {
            return new TestableMovement
            {
                Number = number,
                NotificationId = NotificationId
            };
        }
    }
}
