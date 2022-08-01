namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class CancelMovementsHandlerTests
    {
        private readonly CancelMovementsHandler handler;
        private readonly IMovementRepository repository;
        private readonly IMovementAuditRepository movementAuditRepository;
        private readonly ICapturedMovementFactory capturedMovementFactory;

        private readonly Guid notificationId;
        private const string AnyString = "test";
        private const int CancelMovementCount = 5;

        public CancelMovementsHandlerTests()
        {
            notificationId = Guid.NewGuid();
            var userId = TestIwsContext.UserId;

            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            repository = A.Fake<IMovementRepository>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();
            var userContext = A.Fake<IUserContext>();
            capturedMovementFactory = A.Fake<ICapturedMovementFactory>();

            A.CallTo(() => userContext.UserId).Returns(userId);

            handler = new CancelMovementsHandler(context, repository, movementAuditRepository, userContext,
                capturedMovementFactory);
        }

        [Fact]
        public async Task CancelMovementsHandler_GetsMovementsByIds()
        {
            var request = GetRequest(CancelMovementCount);

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        repository.GetMovementsByIds(notificationId,
                            A<IEnumerable<Guid>>.That.IsSameSequenceAs(request.CancelledMovements.Select(m => m.Id))))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CancelMovementsHandler_LogsAuditAsCancelled()
        {
            var request = GetRequest(CancelMovementCount);

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m => m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Cancelled)))
                .MustHaveHappened(CancelMovementCount, Times.Exactly);
        }

        [Fact]
        public async Task CancelMovementsHandler_ReturnsTrue()
        {
            var request = GetRequest(CancelMovementCount);

            var response = await handler.HandleAsync(request);

            Assert.True(response);
        }

        [Fact]
        public async Task CancelPendingMovementsHandler_ReturnsTrue()
        {
            var request = GetRequest(CancelMovementCount, true);

            var response = await handler.HandleAsync(request);

            Assert.True(response);
        }

        [Fact]
        public async Task CancelPendingMovementsHandler_WithAddedMovements_Captures()
        {
            var request = GetRequestWithAddedMovements();

            var addedMovements = request.AddedMovements.ToList();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        capturedMovementFactory.Create(A<Guid>.That.Matches(guid => guid == notificationId),
                            A<int>.That.Matches(number => addedMovements.Exists(x => x.Number == number)),
                            A<DateTime?>.Ignored, A<DateTime>.Ignored, true))
                .MustHaveHappened(addedMovements.Count, Times.Exactly);
        }

        [Fact]
        public async Task CancelPendingMovementsHandler_WithAddedMovements_AuditsPrenotification()
        {
            var request = GetRequestWithAddedMovements();

            var addedMovements = request.AddedMovements.ToList();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m =>
                                    m.NotificationId == notificationId &&
                                    addedMovements.Exists(x => x.Number == m.ShipmentNumber) &&
                                    m.Type == (int)MovementAuditType.NoPrenotificationReceived)))
                .MustHaveHappened(addedMovements.Count, Times.Exactly);
        }

        [Fact]
        public async Task CancelMovementsHandler_WithAddedMovements_LogsAuditAsCancelled()
        {
            var request = GetRequestWithAddedMovements();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m => m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Cancelled)))
                .MustHaveHappened((request.CancelledMovements.Count() + request.AddedMovements.Count()), Times.Exactly);
        }

        [Fact]
        public async Task CancelMovementsHandler_WithAddedMovements_ReturnsTrue()
        {
            var request = GetRequestWithAddedMovements();

            var response = await handler.HandleAsync(request);

            Assert.True(response);
        }

        private CancelMovements GetRequest(int count, bool isPendingRequest = false)
        {
            var cancelledMovements = new List<MovementData>();
            for (var i = 0; i < count; i++)
            {
                cancelledMovements.Add(new MovementData() { Id = Guid.NewGuid(), Number = i + 1 });
            }

            SetMovements(cancelledMovements, isPendingRequest);

            return new CancelMovements(notificationId, cancelledMovements);
        }

        private void SetMovements(IEnumerable<MovementData> movementDatas, bool isPendingRequest)
        {
            var movements = new List<TestableMovement>();

            foreach (var movementData in movementDatas)
            {
                movements.Add(new TestableMovement
                {
                    Id = movementData.Id,
                    NotificationId = notificationId,
                    Number = movementData.Number,
                    Status = isPendingRequest ? MovementStatus.Captured : MovementStatus.Submitted
                });
            }

            A.CallTo(
                () =>
                    repository.GetMovementsByIds(notificationId,
                        A<IEnumerable<Guid>>.Ignored)).Returns(movements);
        }

        private CancelMovements GetRequestWithAddedMovements()
        {
            var cancelledMovements = new List<MovementData>()
            {
                new MovementData() { Id = Guid.NewGuid(), Number = 1 },
                new MovementData() { Id = Guid.NewGuid(), Number = 2 },
                new MovementData() { Id = Guid.NewGuid(), Number = 3 }
            };

            var addedMovements = new List<AddedCancellableMovement>()
            {
                new AddedCancellableMovement()
                {
                    NotificationId = notificationId,
                    Number = 4,
                    ShipmentDate = SystemTime.Now
                },
                new AddedCancellableMovement()
                {
                    NotificationId = notificationId,
                    Number = 5,
                    ShipmentDate = SystemTime.Now
                }
            };

            var firstAddedMovement = new TestableMovement()
            {
                Id = Guid.NewGuid(),
                NotificationId = notificationId,
                Number = 4,
                Status = MovementStatus.Captured
            };

            var secondAddedMovement = new TestableMovement()
            {
                Id = Guid.NewGuid(),
                NotificationId = notificationId,
                Number = 5,
                Status = MovementStatus.Captured
            };

            A.CallTo(
                () =>
                    capturedMovementFactory.Create(notificationId,
                        A<int>.That.Matches(number => number == firstAddedMovement.Number), null,
                        A<DateTime>.Ignored, true)).Returns(firstAddedMovement);

            A.CallTo(
                () =>
                    capturedMovementFactory.Create(notificationId,
                        A<int>.That.Matches(number => number == secondAddedMovement.Number), null,
                        A<DateTime>.Ignored, true)).Returns(secondAddedMovement);

            var movements = cancelledMovements.Select(cancelled => new TestableMovement
            {
                Id = cancelled.Id,
                NotificationId = notificationId,
                Number = cancelled.Number,
                Status = MovementStatus.Submitted
            }).ToList();

            movements.Add(firstAddedMovement);
            movements.Add(secondAddedMovement);

            A.CallTo(
                () =>
                    repository.GetMovementsByIds(notificationId,
                        A<IEnumerable<Guid>>.Ignored)).Returns(movements);

            return new CancelMovements(notificationId, cancelledMovements, addedMovements);
        }
    }
}
