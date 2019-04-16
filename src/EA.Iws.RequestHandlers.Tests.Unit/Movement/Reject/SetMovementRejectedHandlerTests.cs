namespace EA.Iws.RequestHandlers.Tests.Unit.Movement.Reject
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.Movement.Reject;
    using Requests.Movement.Reject;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetMovementRejectedHandlerTests
    {
        private readonly SetMovementRejectedHandler handler;
        private readonly IRejectMovement rejectMovement;
        private readonly IMovementRepository movementRepository;
        private readonly IFileRepository fileRepository;
        private readonly IMovementAuditRepository movementAuditRepository;

        private readonly Guid notificationId;
        private readonly Guid movementId;
        private readonly DateTime rejectDate;
        private const string NotificatioNumber = "GB12345";
        private const string AnyString = "test";
        private const string FileType = "pdf";

        public SetMovementRejectedHandlerTests()
        {
            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            notificationId = Guid.NewGuid();
            movementId = Guid.NewGuid();
            rejectDate = SystemTime.UtcNow;

            rejectMovement = A.Fake<IRejectMovement>();
            movementRepository = A.Fake<IMovementRepository>();
            fileRepository = A.Fake<IFileRepository>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();
            var notificationRepository = A.Fake<INotificationApplicationRepository>();
            var nameGenerator = new MovementFileNameGenerator(notificationRepository);
            var certificateFactory = new CertificateFactory();
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication() { NotificationNumber = NotificatioNumber });

            A.CallTo(() => fileRepository.Store(A<File>.Ignored)).Returns(Guid.NewGuid());

            A.CallTo(() => userContext.UserId).Returns(TestIwsContext.UserId);

            handler = new SetMovementRejectedHandler(rejectMovement, movementRepository, context, nameGenerator,
                certificateFactory, fileRepository, movementAuditRepository, userContext);
        }

        [Fact]
        public async Task SetMovementRejectedHandler_GetsMovement()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetById(A<Guid>.That.IsEqualTo(request.MovementId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SetMovementRejectedHandler_StoresFile()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => fileRepository.Store(A<File>.That.Matches(f => f.Type == FileType)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SetMovementRejectedHandler_RejectsMovement()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        rejectMovement.Reject(A<Guid>.That.IsEqualTo(movementId),
                            A<DateTime>.That.IsEqualTo(rejectDate), AnyString))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SetMovementRejectedHandler_LogsAuditAsRejected()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m =>
                                    m.NotificationId == notificationId &&
                                    m.Type == (int)MovementAuditType.Rejected)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SetMovementRejectedHandler_SuccessReturnsMovementId()
        {
            var request = GetRequest();

            var response = await handler.HandleAsync(request);

            Assert.Equal(movementId, response);
        }

        private SetMovementRejected GetRequest()
        {
            SetMovement();
            return new SetMovementRejected(movementId, rejectDate, AnyString, new byte[1], FileType);
        }

        private void SetMovement()
        {
            A.CallTo(() => movementRepository.GetById(A<Guid>.That.Matches(id => id == movementId)))
                .Returns(new TestableMovement
                {
                    Id = movementId,
                    NotificationId = notificationId,
                    Status = MovementStatus.Submitted
                });
        }
    }
}
