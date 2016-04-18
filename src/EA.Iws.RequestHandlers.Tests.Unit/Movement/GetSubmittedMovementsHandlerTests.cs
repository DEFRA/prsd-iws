namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.Mappings.Movement;
    using RequestHandlers.Movement.Receive;
    using Requests.Movement.Receive;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetSubmittedMovementsHandlerTests : TestBase, IDisposable
    {
        private readonly GetSubmittedMovementsHandler handler;
        private const int MovementCount = 5;

        public GetSubmittedMovementsHandlerTests()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            var repository = A.Fake<IMovementRepository>();
            var movements = new[]
            {
                GetMovement(1),
                GetMovement(2),
                GetMovement(3),
                GetMovement(4),
                GetMovement(5),
            };

            A.CallTo(() => repository.GetMovementsByStatus(NotificationId, MovementStatus.Submitted))
                .Returns(movements);

            var testMapper = new TestMapper();
            testMapper.AddMapper(new SubmittedMovementMap());
            handler = new GetSubmittedMovementsHandler(repository, testMapper);
        }

        [Fact]
        public async Task ReturnsNotificationMovements()
        {
            var result = await handler.HandleAsync(new GetSubmittedMovements(NotificationId));

            Assert.Equal(MovementCount, result.Count);
        }

        private TestableMovement GetMovement(int number)
        {
            TestableMovement movement = new TestableMovement();
            movement.Id = Guid.NewGuid();
            movement.NotificationId = NotificationId;
            movement.Date = SystemTime.UtcNow.Date.AddDays(-number);
            movement.Number = number;
            return movement;
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}
