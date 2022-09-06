namespace EA.Iws.RequestHandlers.Tests.Unit.Movement.Edit
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.Movement.Edit;
    using Requests.Movement.Edit;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class UpdateMovementDateHandlerTests
    {
        private readonly UpdateMovementDateHandler handler;
        private readonly IMovementRepository repository;
        private readonly IMovementAuditRepository movementAuditRepository;

        private readonly Guid notificationId;
        private const string AnyString = "test";
        private const int ShipmentNumber = 5;

        public UpdateMovementDateHandlerTests()
        {
            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            notificationId = Guid.NewGuid();

            var validator = A.Fake<IUpdatedMovementDateValidator>();
            repository = A.Fake<IMovementRepository>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(TestIwsContext.UserId);

            handler = new UpdateMovementDateHandler(repository, validator, context, movementAuditRepository, userContext);
        }

        [Fact]
        public async Task UpdateMovementDateHandler_GetsMovement()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => repository.GetById(A<Guid>.That.Matches(id => id == request.MovementId)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateMovementDateHandler_LogsAuditAsEdited()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m => m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Edited)))
                .MustHaveHappenedOnceExactly();
        }

        private UpdateMovementDate GetRequest()
        {
            var movementId = Guid.NewGuid();
            var newDate = SystemTime.UtcNow.AddDays(5);

            SetMovement(movementId);

            return new UpdateMovementDate(movementId, newDate);
        }

        private void SetMovement(Guid movementId)
        {
            A.CallTo(() => repository.GetById(A<Guid>.That.Matches(id => id == movementId)))
                .Returns(new TestableMovement
                {
                    Id = movementId,
                    NotificationId = notificationId,
                    Number = ShipmentNumber,
                    Status = MovementStatus.Submitted
                });
        }
    }
}
