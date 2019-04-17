namespace EA.Iws.RequestHandlers.Tests.Unit.Movement.Complete
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using DataAccess;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.Movement.Complete;
    using Requests.Movement.Complete;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class SaveMovementCompletedReceiptHandlerTests
    {
        private readonly SaveMovementCompletedReceiptHandler handler;
        private readonly IMovementRepository movementRepository;
        private readonly IFileRepository fileRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IMovementAuditRepository movementAuditRepository;

        private readonly Guid notificationId;
        private readonly Guid movementId;
        private readonly Guid fileId;
        private const string AnyString = "test";
        private const string NotificationNumber = "GB123";

        public SaveMovementCompletedReceiptHandlerTests()
        {
            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            notificationId = Guid.NewGuid();
            movementId = Guid.NewGuid();
            fileId = Guid.NewGuid();

            var userContext = A.Fake<IUserContext>();
            movementRepository = A.Fake<IMovementRepository>();
            fileRepository = A.Fake<IFileRepository>();
            notificationRepository = A.Fake<INotificationApplicationRepository>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();

            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication() { NotificationNumber = NotificationNumber});

            A.CallTo(() => fileRepository.Store(A<File>.Ignored)).Returns(fileId);

            var nameGenerator = new CertificateOfRecoveryNameGenerator(notificationRepository);
            var certificateFactory = new CertificateFactory();

            handler = new SaveMovementCompletedReceiptHandler(context, fileRepository, movementRepository,
                certificateFactory, nameGenerator, userContext, notificationRepository, movementAuditRepository);
        }

        [Fact]
        public async Task SaveMovementCompletedReceiptHandler_GetsMovement()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => movementRepository.GetById(movementId)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SaveMovementCompletedReceiptHandler_StoresFile()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => fileRepository.Store(A<File>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SaveMovementCompletedReceiptHandler_RecoveryNotification_LogsAuditAsRecovered()
        {
            var request = GetRequest();

            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication() { NotificationNumber = NotificationNumber, NotificationType = NotificationType.Recovery});

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m => m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Recovered)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SaveMovementCompletedReceiptHandler_DisposalNotification_LogsAuditAsDisposed()
        {
            var request = GetRequest();

            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication() { NotificationNumber = NotificationNumber, NotificationType = NotificationType.Disposal });

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m => m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Disposed)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SaveMovementCompletedReceiptHandler_ReturnsFileId()
        {
            var request = GetRequest();

            var response = await handler.HandleAsync(request);

            Assert.Equal(fileId, response);
        }

        private SaveMovementCompletedReceipt GetRequest()
        {
            SetMovement();
            return new SaveMovementCompletedReceipt(movementId, SystemTime.UtcNow, new byte[1], AnyString);
        }

        private void SetMovement()
        {
            var movement = new TestableMovement()
            {
                Id = movementId,
                NotificationId = notificationId,
                Status = MovementStatus.Received
            };

            A.CallTo(() => movementRepository.GetById(movementId)).Returns(movement);
        }
    }
}
