namespace EA.Iws.RequestHandlers.Tests.Unit.Movement.Partial
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using EA.Iws.Core.Shared;
    using EA.Iws.RequestHandlers.Movement.PartialReject;
    using EA.Iws.Requests.Movement.PartialReject;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetMovementPartialRejectedHandlerTests
    {
        private readonly RecordPartialRejectionInternalHandler handler;
        private readonly IPartialRejectionMovement partialRejectionMovement;
        private readonly IMovementRepository movementRepository;

        private readonly Guid notificationId;
        private readonly Guid movementId;
        private readonly DateTime rejectDate;
        private const string NotificatioNumber = "GB123456";
        private const string AnyString = "test";
        private readonly DateTime wasteDate;

        public SetMovementPartialRejectedHandlerTests()
        {
            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            notificationId = Guid.NewGuid();
            movementId = Guid.NewGuid();
            rejectDate = SystemTime.UtcNow;
            wasteDate = SystemTime.UtcNow;

            partialRejectionMovement = A.Fake<IPartialRejectionMovement>();
            movementRepository = A.Fake<IMovementRepository>();
            var notificationRepository = A.Fake<INotificationApplicationRepository>();
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication() { NotificationNumber = NotificatioNumber });

            A.CallTo(() => userContext.UserId).Returns(TestIwsContext.UserId);

            handler = new RecordPartialRejectionInternalHandler(partialRejectionMovement, context);
        }

        private RecordPartialRejectionInternal GetRequest()
        {
            SetPartialRejection();
            return new RecordPartialRejectionInternal(movementId, rejectDate, "Test", 10, ShipmentQuantityUnits.Tonnes, 1, ShipmentQuantityUnits.Tonnes, wasteDate);
        }

        private void SetPartialRejection()
        {
            A.CallTo(() => movementRepository.GetById(A<Guid>.That.Matches(id => id == movementId)))
                .Returns(new TestableMovement
                {
                    Id = movementId,
                    NotificationId = notificationId,
                    Status = MovementStatus.PartiallyRejected
                });
        }

        [Fact]
        public async Task SetMovementRejectedHandler_SuccessReturnsMovementId()
        {
            var request = GetRequest();

            var response = await handler.HandleAsync(request);

            Assert.True(response);
        }
    }
}
