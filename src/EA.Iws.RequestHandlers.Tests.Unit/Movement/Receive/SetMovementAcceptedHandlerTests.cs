namespace EA.Iws.RequestHandlers.Tests.Unit.Movement.Receive
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using DataAccess;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.Movement.Receive;
    using Requests.Movement.Receive;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetMovementAcceptedHandlerTests
    {
        private readonly SetMovementAcceptedHandler handler;
        private readonly IMovementRepository movementRepository;
        private readonly IMovementAuditRepository movementAuditRepository;

        private readonly Guid notificationId;
        private readonly Guid movementId;
        private const string AnyString = "test";

        public SetMovementAcceptedHandlerTests()
        {
            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            notificationId = Guid.NewGuid();
            movementId = Guid.NewGuid();

            movementRepository = A.Fake<IMovementRepository>();
            var userContext = A.Fake<IUserContext>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();

            handler = new SetMovementAcceptedHandler(movementRepository, context, userContext, movementAuditRepository);
        }

        [Fact]
        public async Task SetMovementAcceptedHandler_GetsMovement()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetById(movementId)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SetMovementAcceptedHandler_LogsAuditAsReceived()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m =>
                                        m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Received)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SetMovementAcceptedHandler_ReturnsMovementId()
        {
            var request = GetRequest();

            var response = await handler.HandleAsync(request);

            Assert.Equal(movementId, response);
        }

        private SetMovementAccepted GetRequest()
        {
            SetMovement();

            return new SetMovementAccepted(movementId, Guid.NewGuid(), SystemTime.UtcNow, 1m,
                ShipmentQuantityUnits.Tonnes);
        }

        private void SetMovement()
        {
            var movement = new TestableMovement()
            {
                Id = movementId,
                NotificationId = notificationId,
                Status = MovementStatus.Submitted
            };

            A.CallTo(() => movementRepository.GetById(movementId)).Returns(movement);
        }
    }
}
