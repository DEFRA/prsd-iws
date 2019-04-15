namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
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

        private readonly Guid notificationId;
        private const string AnyString = "test";
        private const int CancelMovementCount = 5;

        public CancelMovementsHandlerTests()
        {
            notificationId = Guid.NewGuid();

            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            repository = A.Fake<IMovementRepository>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();

            handler = new CancelMovementsHandler(context, repository, movementAuditRepository);
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
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task CancelMovementsHandler_LogsAuditAsCancelled()
        {
            var request = GetRequest(CancelMovementCount);

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<Movement>.That.Matches(
                                m =>
                                    m.NotificationId == notificationId), MovementAuditType.Cancelled))
                .MustHaveHappened(Repeated.Exactly.Times(CancelMovementCount));
        }

        [Fact]
        public async Task CancelMovementsHandler_ReturnsTrue()
        {
            var request = GetRequest(CancelMovementCount);

            var response = await handler.HandleAsync(request);

            Assert.True(response);
        }

        private CancelMovements GetRequest(int count)
        {
            var cancelledMovements = new List<MovementData>();
            for (var i = 0; i < count; i++)
            {
                cancelledMovements.Add(new MovementData() { Id = Guid.NewGuid(), Number = i + 1 });
            }

            SetMovements(cancelledMovements);

            return new CancelMovements(notificationId, cancelledMovements);
        }

        private void SetMovements(IEnumerable<MovementData> movementDatas)
        {
            var movements = new List<TestableMovement>();

            foreach (var movementData in movementDatas)
            {
                movements.Add(new TestableMovement
                {
                    Id = movementData.Id,
                    NotificationId = notificationId,
                    Number = movementData.Number,
                    Status = MovementStatus.Submitted
                });
            }

            A.CallTo(
                () =>
                    repository.GetMovementsByIds(notificationId,
                        A<IEnumerable<Guid>>.Ignored)).Returns(movements);
        }
    }
}
