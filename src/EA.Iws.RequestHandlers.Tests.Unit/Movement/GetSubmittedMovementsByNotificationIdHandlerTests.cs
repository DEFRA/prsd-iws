namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using EA.Prsd.Core.Mapper;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.Movement.Receive;
    using Requests.Movement.Receive;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetSubmittedMovementsByNotificationIdHandlerTests : TestBase, IDisposable
    {
        private readonly GetSubmittedMovementsByNotificationIdHandler handler;
        private const int MovementCount = 3;

        public GetSubmittedMovementsByNotificationIdHandlerTests()
        {
            SystemTime.Freeze(DateTime.UtcNow);

            var repository = A.Fake<IMovementRepository>();
            var movements = new[]
            {
                CreateSubmittedMovement(1),
                CreatePendingMovementWithPastDate(2),
                CreatePendingMovementWithFutureDate(3)
            };

            var mapper = A.Fake<IMapper>();

            A.CallTo(() => repository.GetAllActiveMovementsForReceiptAndReceiptRecovery(NotificationId)).Returns(movements);
            handler = new GetSubmittedMovementsByNotificationIdHandler(repository, mapper);
        }

        [Fact]
        public async Task ReturnsNotificationMovements()
        {
            var result = await handler.HandleAsync(new GetSubmittedMovementsByNotificationId(NotificationId));

            Assert.Equal(MovementCount, result.Count);
        }

        private TestableMovement CreateSubmittedMovement(int number)
        {
            TestableMovement movement = new TestableMovement();
            movement.Id = Guid.NewGuid();
            movement.NotificationId = NotificationId;
            movement.Date = SystemTime.UtcNow.Date.AddDays(-number);
            movement.Number = number;
            movement.Status = MovementStatus.Submitted;

            return movement;
        }

        private TestableMovement CreatePendingMovementWithPastDate(int number)
        {
            TestableMovement movement = new TestableMovement();
            movement.Id = Guid.NewGuid();
            movement.NotificationId = NotificationId;
            movement.Date = SystemTime.UtcNow.Date.AddDays(-number);
            movement.Number = number;
            movement.Status = MovementStatus.Submitted;

            return movement;
        }

        private TestableMovement CreatePendingMovementWithFutureDate(int number)
        {
            TestableMovement movement = new TestableMovement();
            movement.Id = Guid.NewGuid();
            movement.NotificationId = NotificationId;
            movement.Date = SystemTime.UtcNow.Date.AddDays(number);
            movement.Number = number;
            movement.Status = MovementStatus.Captured;

            return movement;
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}
