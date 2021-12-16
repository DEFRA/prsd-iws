namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.RequestHandlers.NotificationMovements;
    using EA.Iws.Requests.NotificationMovements;
    using EA.Prsd.Core.Mapper;
    using FakeItEasy;
    using Prsd.Core;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetSubmittedMovementsForRecoveryByNotificationIdHandlerTests : TestBase, IDisposable
    {
        private readonly GetReceivedMovementsHandler handler;
        private const int MovementCount = 3;

        public GetSubmittedMovementsForRecoveryByNotificationIdHandlerTests()
        {
            SystemTime.Freeze(DateTime.UtcNow);

            var repository = A.Fake<IMovementRepository>();
            var notificationRepository = A.Fake<INotificationApplicationRepository>();

            var movements = new[]
            {
                CreateSubmittedMovement(1),
                CreatePendingMovementWithPastDate(2),
                CreatePendingMovementWithFutureDate(3)
            };

            var mapper = A.Fake<IMapper>();

            A.CallTo(() => repository.GetAllActiveMovementsForRecovery(NotificationId)).Returns(movements);
            handler = new GetReceivedMovementsHandler(repository, notificationRepository, mapper);
        }

        [Fact]
        public async Task ReturnsNotificationMovements()
        {
            var result = await handler.HandleAsync(new GetReceivedMovements(NotificationId));

            Assert.Equal(MovementCount, result.MovementDatas.Count);
        }

        private TestableMovement CreateSubmittedMovement(int number)
        {
            TestableMovement movement = new TestableMovement();
            movement.Id = Guid.NewGuid();
            movement.NotificationId = NotificationId;
            movement.Date = SystemTime.UtcNow.Date.AddDays(-number);
            movement.Number = number;
            movement.Status = MovementStatus.Received;

            return movement;
        }

        private TestableMovement CreatePendingMovementWithPastDate(int number)
        {
            TestableMovement movement = new TestableMovement();
            movement.Id = Guid.NewGuid();
            movement.NotificationId = NotificationId;
            movement.Date = SystemTime.UtcNow.Date.AddDays(-number);
            movement.Number = number;
            movement.Status = MovementStatus.Received;

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
